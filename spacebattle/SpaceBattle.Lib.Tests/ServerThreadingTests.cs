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
            var serverThread = IoC.Resolve<ServerThread>("Create And Start Thread", () => { });
            var id = 1;

            var cmd = new Mock<ICommand>();
            cmd.Setup(c => c.Execute()).Verifiable();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetThreadById", (object id) => { return serverThread; }).Execute();
            var hardStopCommandWrapper = IoC.Resolve<ICommand>("Hard Stop The Thread", id, () => { mre.Set(); });

            IoC.Resolve<ICommand>("Send Command", id, cmd.Object).Execute();
            IoC.Resolve<ICommand>("Send Command", id, hardStopCommandWrapper).Execute();
            IoC.Resolve<ICommand>("Send Command", id, cmd.Object).Execute();

            mre.WaitOne(1000);

            Assert.False(serverThread.IsRunning());
            cmd.Verify(c => c.Execute(), Times.Once);
        }

        [Fact]
        public void HardStop_ThrowsException_InvalidId()
        {
            var serverThread = new ServerThread(new BlockingCollection<ICommand>());
            var id = 1;

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetThreadById", (object id) => { return serverThread; }).Execute();
            var hardStopCommandWrapper = IoC.Resolve<ICommand>("Hard Stop The Thread", id, () => { });

            Assert.Throws<ThreadStateException>(hardStopCommandWrapper.Execute);
        }

        [Fact]
        public void SoftStop_Positive()
        {
            var mre = new ManualResetEvent(false);
            var serverThread = IoC.Resolve<ServerThread>("Create And Start Thread", () => { });
            var id = 1;

            var cmd = new Mock<ICommand>();
            cmd.Setup(c => c.Execute()).Verifiable();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetThreadById", (object id) => { return serverThread; }).Execute();

            IoC.Resolve<ICommand>("Send Command", id, cmd.Object).Execute();
            IoC.Resolve<ICommand>("Send Command", id, cmd.Object).Execute();
            IoC.Resolve<ICommand>(
                "Send Command",
                id,
                IoC.Resolve<ICommand>("Soft Stop The Thread", id, () => { mre.Set(); })
            ).Execute();
            IoC.Resolve<ICommand>("Send Command", id, cmd.Object).Execute();
            IoC.Resolve<ICommand>("Send Command", id, cmd.Object).Execute();

            mre.WaitOne(1000);

            Assert.False(serverThread.IsRunning());
            Assert.True(serverThread.IsEmpty());
            cmd.Verify(c => c.Execute(), Times.Exactly(4));
        }

        [Fact]
        public void SoftStop_ThrowsException()
        {
            var serverThread = new ServerThread(new BlockingCollection<ICommand>());
            var id = 1;

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
            var id = 1;
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

            IoC.Resolve<ICommand>("Send Command", id, adaptedSetScopeCommand).Execute();
            IoC.Resolve<ICommand>("Send Command", id, adaptedRegisterCommand).Execute();
            IoC.Resolve<ICommand>("Send Command", id, cmd.Object).Execute();
            IoC.Resolve<ICommand>("Send Command", id, cmdE.Object).Execute();
            IoC.Resolve<ICommand>("Send Command", id, cmd.Object).Execute();
            IoC.Resolve<ICommand>("Send Command", id, hardStopCommandWrapper).Execute();
            IoC.Resolve<ICommand>("Send Command", id, cmd.Object).Execute();

            serverThread.Start();
            mre.WaitOne(1000);

            Assert.Single(q);
            cmd.Verify(m => m.Execute(), Times.Exactly(2));
            handleCommand.Verify(m => m.Execute(), Times.Once());
        }
    }
}
