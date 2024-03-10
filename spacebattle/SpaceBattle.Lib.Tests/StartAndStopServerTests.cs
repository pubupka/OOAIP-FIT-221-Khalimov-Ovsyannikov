using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        }

        [Fact]
        public void StartAndStopServerTestPositive()
        {
            var mockStartCommand = new Mock<ICommand>();
            mockStartCommand.Setup(x => x.Execute()).Verifiable();

            var mockStopCommand = new Mock<ICommand>();
            mockStopCommand.Setup(x => x.Execute()).Verifiable();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Thread",(object[] args)=>
                {
                    return mockStartCommand.Object;
                }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Thread.Start",(object[] args)=>{
                string id = Convert.ToString(args[0]);
                return new StartServerCommand(id);
            }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Start.AllThreads", (object[] args)=>{
                var count = (int)args[0];
                for(int i=0;i<count;i++)
                {
                    IoC.Resolve<ICommand>("Server.Thread.Start", i).Execute();
                    IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Thread.SoftStop." + Convert.ToString(i),
                    (object[] args)=>
                    {
                        return mockStopCommand.Object;
                    }).Execute();
                }
                return new EmptyCommand();
            }).Execute();


            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Stop.AllThreads", (object[] args)=>{
                var count = (int)args[0];
                for(int i=0;i<count;i++)
                {
                    IoC.Resolve<ICommand>("Server.Thread.SoftStop." + Convert.ToString(i)).Execute();
                }
                return new EmptyCommand();
            }).Execute();

            IoC.Resolve<ICommand>("Server.Start.AllThreads", 5).Execute();
            IoC.Resolve<ICommand>("Server.Stop.AllThreads", 5).Execute();

            mockStartCommand.Verify(x => x.Execute(), Times.Exactly(5));
            mockStopCommand.Verify(x => x.Execute(), Times.Exactly(5));
        } 
    }
}
