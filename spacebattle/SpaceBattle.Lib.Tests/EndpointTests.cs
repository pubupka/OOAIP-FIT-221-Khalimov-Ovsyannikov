using Hwdtech;
using Hwdtech.Ioc;
using WebHttp;

namespace SpaceBattle.Lib.Tests
{
    public class EndpointTests
    {
        public EndpointTests()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        }

        [Fact]
        public void EndPoint_Positive()
        {
            var sendCommand = new Mock<ICommand>();
            sendCommand.Setup(cmd => cmd.Execute()).Verifiable();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Build Command From Message",
                (object[] args) =>
                {
                    return new Mock<ICommand>().Object;
                }
            ).Execute();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Get ThreadId By GameId",
                (object[] args) =>
                {
                    return (object)0;
                }
            ).Execute();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Send Command",
                (object[] args) =>
                {
                    return sendCommand.Object;
                }
            ).Execute();

            var ep = new Endpoint();
            ep.ProcessMessage(new MessageContract() { TypeOfCommand = "type", GameId = 1, ItemId = 1});

            sendCommand.Verify(cmd => cmd.Execute(), Times.Once());
        }

        [Fact]
        public void EndPoint_SendCommandFailed()
        {
            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Build Command From Message",
                (object[] args) =>
                {
                    return new Mock<ICommand>().Object;
                }
            ).Execute();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Get ThreadId By GameId",
                (object[] args) =>
                {
                    return (object)0;
                }
            ).Execute();

            var sendCommand = new Mock<ICommand>();
            sendCommand.Setup(cmd => cmd.Execute()).Throws<Exception>();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Send Command",
                (object[] args) =>
                {
                    return sendCommand.Object;
                }
            ).Execute();

            var ep = new Endpoint();
            Assert.Throws<Exception>(() => { ep.ProcessMessage(new MessageContract() { TypeOfCommand = "type", GameId = 1, ItemId = 1}); });
        }

        [Fact]
        public void EndPoint_BuildCommandFromMessage_Failed()
        {
            var buildStrategy = new Mock<IStrategy>();
            buildStrategy.Setup(s => s.Invoke()).Throws<Exception>();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Build Command From Message",
                (object[] args) =>
                {
                    return buildStrategy.Object.Invoke();
                }
            ).Execute();

            var ep = new Endpoint();

            Assert.Throws<Exception>(() => { ep.ProcessMessage(new MessageContract() { TypeOfCommand = "type", GameId = 1, ItemId = 1}); });
        }

        [Fact]
        public void EndPoint_GetThreadIdByGameId_Failed()
        {
            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Build Command From Message",
                (object[] args) =>
                {
                    return new Mock<ICommand>().Object;
                }
            ).Execute();

            var getIdStrategy = new Mock<IStrategy>();
            getIdStrategy.Setup(s => s.Invoke()).Throws<Exception>();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Get ThreadId By GameId",
                (object[] args) =>
                {
                    return getIdStrategy.Object.Invoke();
                }
            ).Execute();

            var ep = new Endpoint();

            Assert.Throws<Exception>(() => { ep.ProcessMessage(new MessageContract() { TypeOfCommand = "type", GameId = 1, ItemId = 1}); });
        }
    }
}
