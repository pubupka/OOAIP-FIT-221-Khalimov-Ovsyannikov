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

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Get Thread Id By Game Id",
                (object[] args) =>
                {
                    return (object)0;
                }
            ).Execute();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Build Command From Message",
                (object[] args) =>
                {
                    return new Mock<ICommand>().Object;
                }
            ).Execute();
        }

        [Fact]
        public void EndPoint_Positive()
        {
            var sendCommand = new Mock<ICommand>();
            sendCommand.Setup(cmd => cmd.Execute()).Verifiable();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Send Command",
                (object[] args) =>
                {
                    return sendCommand.Object;
                }
            ).Execute();

            var ep = new Endpoint();
            ep.ProcessMessage(new MessageContract());

            sendCommand.Verify(cmd => cmd.Execute(), Times.Once());
        }

        [Fact]
        public void EndPoint_SendCommandFailed()
        {
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
            Assert.Throws<Exception>(() => { ep.ProcessMessage(new MessageContract()); });
        }
    }
}
