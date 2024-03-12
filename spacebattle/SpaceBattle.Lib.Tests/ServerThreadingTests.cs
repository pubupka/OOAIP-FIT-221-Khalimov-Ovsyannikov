using Hwdtech.Ioc;
using Hwdtech;
using System.Collections.Concurrent;

namespace SpaceBattle.Lib.Tests
{
    public class ServerThreadingTests
    {
        /*

        1) ХардСтоп нормальный
        2) Хардстоп исключение
        3) Софтстоп нормальный
        4) Софтстоп исключение

        */
        public ServerThreadingTests()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
            new InitServerThreadDependenciesCommand().Execute();
        }

        [Fact]
        public void HardStop_Positive()
        {
            var mre = new ManualResetEvent(false);
            var q = new BlockingCollection<ICommand>();
            var serverThread = new ServerThread(q);
            var id = serverThread.GetId();

            var cmd = new Mock<ICommand>();
            cmd.Setup(c => c.Execute()).Verifiable();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetThreadById", (object id) => { return serverThread; }).Execute();
            var hardStopCommandWrapper = IoC.Resolve<ICommand>("Hard Stop The Thread", id, () => { mre.Set();} );
            
            serverThread.AddCommand(cmd.Object);
            serverThread.AddCommand(hardStopCommandWrapper);
            serverThread.AddCommand(cmd.Object);

            serverThread.Start();
            mre.WaitOne();

            Assert.Single(q);
            Assert.True(!serverThread.IsRunning());
            cmd.Verify(c => c.Execute(), Times.Once);
        }

        [Fact]
        public void HardStop_ThrowsException()
        {
            // var serverThread = IoC.Resolve<ServerThread>("Create And Start Thread");
            // var id = 123;  // какой-попало айди
            
            // IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetThreadById", (object id) => { return serverThread; }).Execute();
            // var hardStopCommandWrapper = IoC.Resolve<ICommand>("Hard Stop The Thread", id, () => { } );

            // Assert.Throws<ThreadStateException>(hardStopCommandWrapper.Execute);
        }

        [Fact]
        public void SoftStop_Positive()
        {
            // var serverThread = IoC.Resolve<ServerThread>("Create And Start Thread");
            // var id = serverThread.GetId();
            // var isDone = false;
            // void ActionAfterStop() 
            // {
            //     isDone = true;
            // }

            // IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetThreadById", (object id) => { return serverThread; }).Execute();
            // var hardStopCommandWrapper = IoC.Resolve<ICommand>("Hard Stop The Thread", id, () => { ActionAfterStop();} );
            
            // hardStopCommandWrapper.Execute();

            // Assert.True(isDone);
            // Assert.True(!serverThread.IsRunning());
        }
    }
}
