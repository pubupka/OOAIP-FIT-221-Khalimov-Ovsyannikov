using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests
{
    public class DeleteGameCommandStrategyTests
    {
        public DeleteGameCommandStrategyTests()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        }

        [Fact]
        public void DeleteGameCommandStrategyPositive()
        {
            var mockGame = new InjectCommand(new EmptyCommand());
            var emptyCmd = new EmptyCommand();
            var dictOfgames = new Dictionary<string, IInjectable>() { { "asdfg", mockGame } };
            var dictOfScopes = new Dictionary<string, object>() { { "asdfg", 0 } };

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.DeleteGameCommandStrategy", (object[] args) => new DeleteGameCommandStrategy().Invoke(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Dict", (object[] args) => dictOfgames).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Scope.Dict", (object[] args) => dictOfScopes).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.EmptyCommand", (object[] args) => emptyCmd).Execute();

            Assert.Single(dictOfgames);
            Assert.Single(dictOfScopes);

            IoC.Resolve<ICommand>("Game.DeleteGameCommandStrategy", "asdfg").Execute();

            Assert.Single(dictOfgames);
            Assert.Empty(dictOfScopes);
        }

        [Fact]
        public void DeleteGameCommandStrategyDeletingByNonExistenId()
        {
            var mockGame = new InjectCommand(new EmptyCommand());
            var emptyCmd = new EmptyCommand();
            var dictOfgames = new Dictionary<string, IInjectable>();
            var dictOfScopes = new Dictionary<string, object>() { { "asdfg", 0 } };

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.DeleteGameCommandStrategy", (object[] args) => new DeleteGameCommandStrategy().Invoke(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Dict", (object[] args) => dictOfgames).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Scope.Dict", (object[] args) => dictOfScopes).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.EmptyCommand", (object[] args) => emptyCmd).Execute();

            Assert.Throws<KeyNotFoundException>(() => { IoC.Resolve<ICommand>("Game.DeleteGameCommandStrategy", "asdfg").Execute(); });
        }

        [Fact]
        public void DeleteGameCommandStrategyCantChangeGameCommandToEmprtyCommand()
        {
            var mockGame = new Mock<IInjectable>();
            mockGame.Setup(x => x.Inject(It.IsAny<ICommand>())).Throws(new Exception());
            var emptyCmd = new EmptyCommand();
            var dictOfgames = new Dictionary<string, IInjectable>() { { "asdfg", mockGame.Object } };
            var dictOfScopes = new Dictionary<string, object>();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.DeleteGameCommandStrategy", (object[] args) => new DeleteGameCommandStrategy().Invoke(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Dict", (object[] args) => dictOfgames).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Scope.Dict", (object[] args) => dictOfScopes).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.EmptyCommand", (object[] args) => emptyCmd).Execute();

            Assert.Throws<Exception>(() => { IoC.Resolve<ICommand>("Game.DeleteGameCommandStrategy", "asdfg").Execute(); });
        }
    }
}
