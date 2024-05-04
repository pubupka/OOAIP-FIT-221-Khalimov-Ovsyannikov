using System.Collections.Concurrent;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests
{
    public class СreateGameCommandStrategyTests
    {
        public СreateGameCommandStrategyTests()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        }

        [Fact]
        public void CreateGameCommandStrategyPositiveTest()
        {
            var mockCmd = new Mock<ICommand>();
            mockCmd.Setup(x => x.Execute()).Verifiable();

            var gameDict = new Dictionary<string, ICommand>();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.CreateGameCommandStrategy", (object[] args) => new CreateGameCommandStrategy().Invoke(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Dict", (object[] args) => gameDict).Execute();

            var threadCollection = new BlockingCollection<ICommand>();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Thread.Games", (object[] args) => threadCollection).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Scope.New", (object[] args) => (object)0).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Queue.New", (object[] args) => new Queue<ICommand>()).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command", (object[] args) => mockCmd.Object).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Inject", (object[] args) => new InjectCommand((ICommand)args[0])).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.Concurrent.Repeat", (object[] args) => new RepeatConcurrentCommand((ICommand)args[0])).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Macro", (object[] args) =>
            {
                var cmds = (List<ICommand>)args[0];
                return new MacroComand(cmds);
            }).Execute();

            var gameCmd = IoC.Resolve<ICommand>("Game.CreateGameCommandStrategy", "asdfg", IoC.Resolve<object>("Scopes.Current"), 5);
            Assert.Equal(gameCmd, gameDict["asdfg"]);

            mockCmd.Verify(x => x.Execute(), Times.Never());
            Assert.Empty(threadCollection);

            threadCollection.Add(gameCmd);
            Assert.Single(threadCollection);

            threadCollection.Take().Execute();
            threadCollection.Take().Execute();

            Assert.Single(threadCollection);
            mockCmd.Verify(x => x.Execute(), Times.Exactly(2));
        }
    }
}
