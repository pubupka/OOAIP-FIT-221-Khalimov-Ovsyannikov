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

            var cmd = new Mock<ICommand>();
            cmd.Setup(x => x.Execute());

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Start",(object[] args)=>
            {
                return cmd.Object;
            }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register","Server.Stop", (object[] args)=>
            {
                return cmd.Object;
            }).Execute();
        }
        [Fact]
        public void RunMainCommandTest()
        {
            var count = 5;

            var inputManager = new StringReader("Enter");
            Console.SetIn(inputManager);
            var outputManager = new StringWriter();
            Console.SetOut(outputManager);

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