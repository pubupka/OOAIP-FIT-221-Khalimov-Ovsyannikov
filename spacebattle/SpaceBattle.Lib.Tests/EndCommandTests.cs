using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests
{
    public class EndCommandTests
    {
        private static void EndCommandStartTest()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Inject", (object[] args) =>
            {
                var target = (IInjectable)args[0];
                var injectedCommand = (ICommand)args[1];
                target.Inject(injectedCommand);
                return target;
            }).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.EmptyCommand", (object[] args) => new EmptyCommand()).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.EndMovement", (object[] args) => { return new EndMovementCommand((IEndable)args[0]); }).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.UObject.DeleteProperty", (object[] args) =>
            {
                var target = (IUObject)args[0];
                var properties = (List<string>)args[1];
                properties.ForEach(prop => target.DeleteProperty(prop));
                return "";
            }).Execute();
        }

        [Fact]
        public void EndMovementCommandTest()
        {
            EndCommandStartTest();
            var mockEndable = new Mock<IEndable>();
            var mockCommand = new Mock<ICommand>();
            var injectCommand = new InjectCommand(mockCommand.Object);
            var target = new Mock<IUObject>();
            var keys = new List<string>() { "Movement" };
            var characteristics = new Dictionary<string, object>();

            target.Setup(t => t.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Callback<string, object>((key, value) => characteristics.Add(key, value));
            target.Setup(t => t.DeleteProperty(It.IsAny<string>())).Callback<string>((string key) => characteristics.Remove(key));
            target.Setup(t => t.GetProperty(It.IsAny<string>())).Returns((string key) => characteristics[key]);
            target.Object.SetProperty("Movement", 1);

            mockEndable.SetupGet(e => e.command).Returns(injectCommand);
            mockEndable.SetupGet(e => e.target).Returns(target.Object);
            mockEndable.SetupGet(e => e.property).Returns(keys);

            IoC.Resolve<SpaceBattle.Lib.EndMovementCommand>("Game.Command.EndMovement", mockEndable.Object).Execute();
            Assert.Throws<System.Collections.Generic.KeyNotFoundException>(() => target.Object.GetProperty("Movement"));
        }

        [Fact]
        public void InjectCommandTest()
        {
            EndCommandStartTest();

            var mockCommand = new Mock<ICommand>();
            mockCommand.Setup(x => x.Execute()).Verifiable();

            var injectCommand = new InjectCommand(mockCommand.Object);
            injectCommand.Inject(IoC.Resolve<ICommand>("Game.Command.EmptyCommand"));
            injectCommand.Execute();

            mockCommand.Verify(m => m.Execute(), Times.Never());
        }
    }
}
