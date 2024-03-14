using System.Collections.Concurrent;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests
{
    public class ServerThreadingTests
    {
        /*

        1) ХардСтоп нормальный
        2) Хардстоп исключение
        3) Софтстоп нормальный
        4) Софтстоп исключение
        5) Выброшенное командой исключение не останавливает поток

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
            var hardStopCommandWrapper = IoC.Resolve<ICommand>("Hard Stop The Thread", id, () => { mre.Set(); });

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
        public void HardStop_ThrowsException_InvalidId()
        {
            var serverThread = new ServerThread(new BlockingCollection<ICommand>());
            var id = 123;  // какой-попало айди

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetThreadById", (object id) => { return serverThread; }).Execute();
            var hardStopCommandWrapper = IoC.Resolve<ICommand>("Hard Stop The Thread", id, () => { });

            Assert.Throws<ThreadStateException>(hardStopCommandWrapper.Execute);
        }

        [Fact]
        public void SoftStop_Positive()  // IMPOSTER
        {
            var q = new BlockingCollection<ICommand>();
            var serverThread = IoC.Resolve<ServerThread>("Create And Start Thread", () =>
            {

            });
            var id = serverThread.GetId();

            var cmd = new Mock<ICommand>();
            cmd.Setup(c => c.Execute()).Verifiable();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetThreadById", (object id) => { return serverThread; }).Execute();
            var mre = new ManualResetEvent(false);

            serverThread.AddCommand(cmd.Object);
            serverThread.AddCommand(cmd.Object);

            serverThread.AddCommand(IoC.Resolve<ICommand>("Soft Stop The Thread", id, () => { mre.Set(); }));
            serverThread.AddCommand(cmd.Object);
            serverThread.AddCommand(cmd.Object);

            mre.WaitOne();
            Thread.Sleep(100);

            Assert.Empty(q);
            Assert.True(!serverThread.IsRunning());
            cmd.Verify(c => c.Execute(), Times.Exactly(4));
        }

        [Fact]
        public void SoftStop_ThrowsException_InvalidId()
        {
            var serverThread = new ServerThread(new BlockingCollection<ICommand>());
            var id = 123;  // какой-попало айди

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetThreadById", (object id) => { return serverThread; }).Execute();
            var softStopCommandWrapper = IoC.Resolve<ICommand>("Soft Stop The Thread", id, () => { });

            Assert.Throws<ThreadStateException>(softStopCommandWrapper.Execute);
        }

        [Fact]
        public void ExceptionCommandShouldNotStopThread()
        {
            var mre = new ManualResetEvent(false);
            var q = new BlockingCollection<ICommand>();
            var serverThread = new ServerThread(q);
            var cmd = new Mock<ICommand>();
            var id = serverThread.GetId();
            cmd.Setup(m => m.Execute()).Verifiable();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetThreadById", (object id) => { return serverThread; }).Execute();
            var hardStopCommandWrapper = IoC.Resolve<ICommand>("Hard Stop The Thread", id, () => { mre.Set(); });

            var handleCommand = new Mock<ICommand>();
            handleCommand.Setup(m => m.Execute()).Verifiable();

            var cmdE = new Mock<ICommand>();
            cmdE.Setup(m => m.Execute()).Throws<Exception>().Verifiable();

            var setScopeCommand = IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set",
                    IoC.Resolve<object>("Scopes.New",
                        IoC.Resolve<object>("Scopes.Root")
                    )
                );

            var adaptedSetScopeCommand = new HwdICommandToICommandAdapter(setScopeCommand);

            var adaptedRegisterCommand = new HwdICommandToICommandAdapter(
                IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Exception.Handle", (object[] args) => handleCommand.Object)
            );

            q.Add(adaptedSetScopeCommand);
            q.Add(adaptedRegisterCommand);
            q.Add(cmd.Object);
            q.Add(cmdE.Object);
            q.Add(cmd.Object);
            q.Add(hardStopCommandWrapper);
            q.Add(cmd.Object);

            serverThread.Start();
            mre.WaitOne();

            Assert.Single(q);
            cmd.Verify(m => m.Execute(), Times.Exactly(2));
            handleCommand.Verify(m => m.Execute(), Times.Once());
        }

        [Fact]
        public void SendCommandPositive()
        {
            var q = new BlockingCollection<ICommand>();
            var serverThread = new ServerThread(q);
            var id = serverThread.GetId();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetThreadById", (object id) => { return serverThread; }).Execute();

            IoC.Resolve<object>("Send Command", id, new Mock<ICommand>().Object);

            Assert.Single(q);
        }
    }
}
