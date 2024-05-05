using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceBattle.Lib.Tests
{
    public class CreateGameScopeStrategyTests
    {
        public CreateGameScopeStrategyTests()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        }

        [Fact]
        public void CreateGameScopeStrategyPositive()
        {
            var quantum = 5;
            var gameId = "asdfg";
            var mockCmd = new Mock<ICommand>();
            var uobjectId = 1;
            var uobject = new Mock<IUObject>();
            var uobjectDict = new Dictionary<int, IUObject>() {{1, uobject.Object}};

            var gameScope = IoC.Resolve<object>("Game.CreateNewScope", gameId, IoC.Resolve<object>("Scopes.Current"), quantum).Execute();
            Assert.Fails(IoC.Resolve<int>("Game.Get.Time.Quantum"));
            Assert.Fails(IoC.Resolve<ICommand>("Game.Queue.Inqueue", gameId, mockCmd.Object));
            Assert.Fails(IoC.Resolve<ICommand>("Game.Queue.Dequeue", gameId));
            Assert.Fails(IoC.Resolve<IUObject>("Game.Get.UObject", uobjectId));
            Assert.Fails(IoC.Resolve<ICommand>("Game.Delete.UObject", uobjectId));

            var id = (string)args[0];
            var cmd = (ICommand)args[1];

            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", gameScope).Execute();

            Assert.Equal(IoC.Resolve<ICommand>("Game.Get.Time.Quantum"), quantum);
            IoC.Resolve<ICommand>("Game.Queue.Inqueue", gameId, mockCmd.Object).Execute();
            Assert.Equal(IoC.Resolve<ICommand>("Game.Queue.Dequeue", gameId).Execute(), mockCmd.Object);
            Assert.Equal(IoC.Resolve<IUObject>("Game.Get.UObject", uobjectId), uobject.Object);
            IoC.Resolve<ICommand>("Game.Delete.UObject", uobjectId).Execute();
            Assert.Empty(uobjectDict);
        }
    }
}