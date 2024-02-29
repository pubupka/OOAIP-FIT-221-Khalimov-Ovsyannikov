using System.Collections.Concurrent;
using Hwdtech.Ioc;
using Hwdtech;

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
        public void HardStopTest()
        {
            var serverThread = IoC.Resolve<ServerThread>("Create And Start Thread");
            var id = serverThread.GetId();
            var isDone = false;
            void ActionAfterStop() 
            {
                isDone = true;
            }
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetThreadById", (object id) => { return serverThread; }).Execute();
            var hardStopCommandWrapper = IoC.Resolve<ICommand>("Hard Stop The Thread", id, () => { ActionAfterStop();} );
            
            hardStopCommandWrapper.Execute();

            Assert.True(isDone);
            Assert.True(!serverThread.IsRunning());
        }
    }
}
