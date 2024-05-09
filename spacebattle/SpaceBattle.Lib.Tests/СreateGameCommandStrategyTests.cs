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

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Scope.New", (object[] args) => (object)0).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Queue.New", (object[] args) => new Queue<ICommand>()).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command", (object[] args) => mockCmd.Object).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Inject", (object[] args) => new InjectCommand((ICommand)args[0])).Execute();

            var gameCmd = IoC.Resolve<ICommand>("Game.CreateGameCommandStrategy", "asdfg", IoC.Resolve<object>("Scopes.Current"), 5);
            Assert.Equal(gameCmd, gameDict["asdfg"]);
            gameCmd.Execute();

            mockCmd.Verify(x => x.Execute(), Times.Once());
        }

        [Fact]
        public void CreateGameCommandStrategyCreatedGameCommandCantBeExecuted()
        {
            var mockCmd = new Mock<ICommand>();
            mockCmd.Setup(x => x.Execute()).Throws(new Exception());

            var gameDict = new Dictionary<string, ICommand>();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.CreateGameCommandStrategy", (object[] args) => new CreateGameCommandStrategy().Invoke(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Dict", (object[] args) => gameDict).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Scope.New", (object[] args) => (object)0).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Queue.New", (object[] args) => new Queue<ICommand>()).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command", (object[] args) => mockCmd.Object).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Inject", (object[] args) => new InjectCommand((ICommand)args[0])).Execute();

            var gameCmd = IoC.Resolve<ICommand>("Game.CreateGameCommandStrategy", "asdfg", IoC.Resolve<object>("Scopes.Current"), 5);

            Assert.Throws<Exception>(()=>gameCmd.Execute());
        }
    }
}
