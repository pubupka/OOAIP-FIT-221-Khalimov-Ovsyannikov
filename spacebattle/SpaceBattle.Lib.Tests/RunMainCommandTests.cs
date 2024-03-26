using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests
{
    public class RunMainCommandTests
    {
        public RunMainCommandTests()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();

            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New",
            IoC.Resolve<object>("Scopes.Root"))).Execute();
        }
        [Fact]
        public void RunMainCommandTest()
        {
            var count = 5;
            var barrier = new Barrier(count+1);

            var inputManager = new StringReader("Enter");
            Console.SetIn(inputManager);
            var outputManager = new StringWriter();
            Console.SetOut(outputManager);

            var cmd = new Mock<ICommand>();
            cmd.Setup(x => x.Execute());

             IoC.Resolve<Hwdtech.ICommand>("IoC.Register","Server.MakeBarrier", (object[] args) =>{
                return barrier;
            }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Start",(object[] args)=>
            {
                return cmd.Object;
            }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register","Server.Stop", (object[] args)=>
            {
                barrier.RemoveParticipants(count);
                return cmd.Object;
            }).Execute();

            var maincmd = new RunMainCommand(count);
            maincmd.Execute();
            var output = outputManager.ToString();

            Assert.Contains($"Запуск {count}-ти поточного сервера", output);
            Assert.Contains($"{count}-ти поточный сервер запущен", output);
            Assert.Contains($"Остановка {count}-ти поточного сервера", output);
            Assert.Contains($"{count}-ти поточный сервер остановлен", output);
        }
    }
}
