using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests
{
    public class GameCommandTests
    {
        public GameCommandTests()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
        }

        [Fact]
        public void GameCommand_Execute_Positive()
        {
            var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();
            var q = new Queue<ICommand>();
            var getQuantStrategy = new Mock<IStrategy>();
            getQuantStrategy.Setup(s => s.Invoke()).Returns(30);

            var cmd1 = new Mock<ICommand>();
            cmd1.Setup(c => c.Execute()).Callback(() => { getQuantStrategy.Setup(s => s.Invoke()).Returns(0); }).Verifiable();

            var cmd2 = new Mock<ICommand>();
            cmd2.Setup(c => c.Execute()).Callback(() => { }).Verifiable();

            q.Enqueue(cmd1.Object);
            q.Enqueue(cmd2.Object);

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetQuant", (object[] args) => getQuantStrategy.Object.Invoke()).Execute();

            var gameCommand = new GameCommand(q, scope);
            gameCommand.Execute();

            cmd1.Verify(c => c.Execute(), Times.Once());
            cmd2.Verify(c => c.Execute(), Times.Never());
        }

        [Fact]
        public void GameCommand_Execute_ThrowsException_HandledBySpecificHandler()
        {
            var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();
            var q = new Queue<ICommand>();

            var cmd1 = new Mock<ICommand>();
            cmd1.Setup(c => c.Execute()).Throws<FileNotFoundException>();

            var cmd2 = new Mock<ICommand>();
            cmd2.Setup(c => c.Execute()).Callback(() => { }).Verifiable();

            q.Enqueue(cmd1.Object);
            q.Enqueue(cmd2.Object);

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetQuant", (object[] args) => (object)30).Execute();

            var handler = new Mock<IHandler>();
            handler.Setup(h => h.Handle()).Callback(() => { }).Verifiable();

            var tree = new Dictionary<Type, object>() {
                    { cmd1.Object.GetType(), new Dictionary<Type, string>() }
            };
            new InitTreeCommand(tree).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "FindHandler", (object[] args) =>
            {
                return handler.Object;
            }).Execute();

            var gameCommand = new GameCommand(q, scope);
            gameCommand.Execute();

            handler.Verify(h => h.Handle(), Times.Once());
            cmd2.Verify(c => c.Execute(), Times.Once());
        }

        [Fact]
        public void GameCommand_Execute_ThrowsException_HandledByDefaultHandler()
        {
            var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();
            var q = new Queue<ICommand>();

            var cmd1 = new Mock<ICommand>();
            cmd1.Setup(c => c.Execute()).Throws<FileNotFoundException>();

            var cmd2 = new Mock<ICommand>();
            cmd2.Setup(c => c.Execute()).Callback(() => { }).Verifiable();

            q.Enqueue(cmd1.Object);
            q.Enqueue(cmd2.Object);

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetQuant", (object[] args) => (object)30).Execute();

            new InitTreeCommand(new Dictionary<Type, object>()).Execute();

            var gameCommand = new GameCommand(q, scope);
            
            Assert.Throws<FileNotFoundException>(gameCommand.Execute);
            cmd2.Verify(c => c.Execute(), Times.Never());
        }
    }
}
