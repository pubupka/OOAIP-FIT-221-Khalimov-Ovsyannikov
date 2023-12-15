using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests
{
    public class LongOperationTests
    {
        public LongOperationTests()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();

            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        }

        [Fact]
        public void LongOperationTestPositive()
        {
            var mockCommand = new Mock<ICommand>();
            mockCommand.Setup(x => x.Execute()).Verifiable();

            var repeatCommand = new Mock<ICommand>();
            repeatCommand.Setup(m => m.Execute());

            var name = "Movement";
            var mockUObject = new Mock<IUObject>();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command." + name, (object[] args) => mockCommand.Object).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Macro", (object[] args) => mockCommand.Object).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Repeat", (object[] args) => repeatCommand.Object).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Inject", (object[] args) => mockCommand.Object).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Operation." + name, (object[] args) => { return new LongOperationStrategy(name, (IUObject)args[0]).Invoke(); }).Execute();

            IoC.Resolve<ICommand>("Game.Operation." + name, mockUObject.Object).Execute();
            mockCommand.Verify();
        }

        [Fact]
        public void LongOperationTestNegative()
        {
            var mockCommand = new Mock<ICommand>();
            mockCommand.Setup(m => m.Execute());

            var repeatCommand = new Mock<ICommand>();
            repeatCommand.Setup(m => m.Execute());

            var unactiveMockCommand = new Mock<ICommand>();
            unactiveMockCommand.Setup(u => u.Execute()).Verifiable();

            var name = "Movement";
            var mockUObject = new Mock<IUObject>();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command." + name, (object[] args) => mockCommand.Object).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Macro", (object[] args) => mockCommand.Object).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Repeat", (object[] args) => repeatCommand.Object).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Inject", (object[] args) => mockCommand.Object).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Operation." + name, (object[] args) => { return new LongOperationStrategy(name, (IUObject)args[0]).Invoke(); }).Execute();

            IoC.Resolve<ICommand>("Game.Operation." + name, mockUObject.Object).Execute();
            unactiveMockCommand.Verify(x => x.Execute(), Times.Never);
        }
    }
}
