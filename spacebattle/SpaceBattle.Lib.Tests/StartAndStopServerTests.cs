using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests
{
    public class StartAndStopServerTests
    {
        public StartAndStopServerTests()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();

            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New",
            IoC.Resolve<object>("Scopes.Root"))).Execute();
            new InitStartAndStopServerCommand().Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Threads.Collection." + "-1", (object[] args) =>
            {
                var thread = new EmptyCommand();
                return thread;
            }).Execute();
        }

        [Fact]
        public void StartAndStopServerTestPositive()
        {
                        
            ManualResetEvent mre = new ManualResetEvent(false);
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ManualResetEvent.Set", (object[] args)=>{
                mre.Set();
                return new EmptyCommand();}).Execute();

            var mockStartCommand = new Mock<ICommand>();
            mockStartCommand.Setup(x => x.Execute()).Verifiable();

            var mockStopCommand = new Mock<ICommand>();
            mockStopCommand.Setup(x => x.Execute()).Verifiable();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Thread", (object[] args) =>
                {
                    return mockStartCommand.Object;
                }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Thread.SoftStop",
                    (object[] args) =>
                    {
                        var thread = IoC.Resolve<ICommand>("Server.Threads.Collection." + (string)args[0]);
                        return mockStopCommand.Object;
                    }).Execute();

            IoC.Resolve<ICommand>("Server.Start.AllThreads", 5).Execute();

            IoC.Resolve<ICommand>("Server.Stop.AllThreads", 5).Execute();

            mre.WaitOne();

            mockStartCommand.Verify(x => x.Execute(), Times.Exactly(5));
            mockStopCommand.Verify(x => x.Execute(), Times.Exactly(5));

            IoC.Resolve<ICommand>("Server.Thread.SoftStop", "-1").Execute();
            mockStopCommand.Verify(x => x.Execute(), Times.Exactly(6));
        }

        [Fact]
        public void StopServerByIdTest()
        {        
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ManualResetEvent.Set", (object[] args)=>{
                return new EmptyCommand();}).Execute();

            var mockStartCommand = new Mock<ICommand>();
            mockStartCommand.Setup(x => x.Execute()).Verifiable();

            var mockStopCommand = new Mock<ICommand>();
            mockStopCommand.Setup(x => x.Execute()).Verifiable();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Thread", (object[] args) =>
                {
                    return mockStartCommand.Object;
                }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Thread.SoftStop",
                    (object[] args) =>
                    {
                        var thread = IoC.Resolve<ICommand>("Server.Threads.Collection." + (string)args[0]);
                        return mockStopCommand.Object;
                    }).Execute();

            IoC.Resolve<ICommand>("Server.Start.AllThreads", 5).Execute();
            new StopServerCommand("3").Execute();

            mockStartCommand.Verify(x => x.Execute(), Times.Exactly(5));
            mockStopCommand.Verify(x => x.Execute(), Times.Once());
        }

        [Fact]
        public void StopUnexistingServer()
        {       
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ManualResetEvent.Set", (object[] args)=>{
                return new EmptyCommand();}).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Thread.SoftStop",
                (object[] args) =>
                    {
                        return new EmptyCommand();
                    }).Execute();

            var cmd = IoC.Resolve<ICommand>("Server.Thread.Stop", "");
            var cmd2 = new StopServerCommand(null);
            
            Assert.Throws<Exception>(cmd.Execute);
            Assert.Throws<Exception>(cmd2.Execute);
        }
    }
}
