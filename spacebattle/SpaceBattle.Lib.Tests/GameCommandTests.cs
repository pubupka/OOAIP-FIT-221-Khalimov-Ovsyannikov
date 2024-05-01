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
        public void GameCommand_Execute_ThrowsException()
        {
            var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();
            var q = new Queue<ICommand>();

            var cmd1 = new Mock<ICommand>();
            cmd1.Setup(c => c.Execute()).Throws<Exception>();

            var cmd2 = new Mock<ICommand>();
            cmd2.Setup(c => c.Execute()).Callback(() => { }).Verifiable();

            q.Enqueue(cmd1.Object);
            q.Enqueue(cmd2.Object);

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetQuant", (object[] args) => (object)30).Execute();

            var handleCommand = new Mock<ICommand>();
            handleCommand.Setup(c => c.Execute()).Verifiable();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Exception.Handle", (object[] args) => handleCommand.Object).Execute();

            var gameCommand = new GameCommand(q, scope);
            gameCommand.Execute();

            handleCommand.Verify(c => c.Execute(), Times.Once());
            cmd2.Verify(c => c.Execute(), Times.Once());
        }
    }
}
