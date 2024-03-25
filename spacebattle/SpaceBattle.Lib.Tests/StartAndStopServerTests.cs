using Hwdtech;
using Hwdtech.Ioc;
using System.Collections.Concurrent;

namespace SpaceBattle.Lib.Tests
{
    public class StartAndStopServerTests
    {
        public StartAndStopServerTests()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();

            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New",
            IoC.Resolve<object>("Scopes.Root"))).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Start",(object[] args)=>
            {
                return new StartServerCommand((int)args[0]);
            }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register","Server.Stop", (object[] args)=>
            {
                return new StopServerCommand();
            }).Execute();
        }

        [Fact]
        public void StartAndStopServerTestPositive()
        {
            var barrier = new Barrier(6); // 5 потоков+мейн
            var mockStartCommand = new Mock<ICommand>();
            mockStartCommand.Setup(x => x.Execute()).Verifiable();

            var mockStopCommand = new Mock<ICommand>();
            mockStopCommand.Setup(x => x.Execute()).Verifiable();

            var threadsCollection = new ConcurrentDictionary<int, BlockingCollection<ICommand>>();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Thread.Start", (object[] args)=>
            {
                var index = (int)args[0];
                threadsCollection.AddOrUpdate(index, new BlockingCollection<ICommand>(), (k,v) => v);

                return mockStartCommand.Object;
            }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register","Server.Take.Threads", (object[] args)=>
            {
                return threadsCollection;
            }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Action", (object[] args)=>
            {
                return ()=>{barrier.RemoveParticipant();};
            }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.SoftStopCommand", (object[] args)=>
            {
                var actCommand = new ActionCommand((Action) args[0]);
                var cmds = new ICommand[2]{mockStopCommand.Object, actCommand};
                return new MacroCommand(cmds);
            }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.SendCommand", (object[] args)=>
            {
                var index = (int)args[0];
                var cmd = (ICommand)args[1];
                threadsCollection[index].Add(cmd);
                return cmd;
            }).Execute();

            IoC.Resolve<ICommand>("Server.Start", 5).Execute();
            IoC.Resolve<ICommand>("Server.Stop").Execute();
            barrier.SignalAndWait();
           
            mockStartCommand.Verify(x => x.Execute(), Times.Exactly(5));
            mockStopCommand.Verify(x => x.Execute(), Times.Exactly(5));
        }
    }
}
