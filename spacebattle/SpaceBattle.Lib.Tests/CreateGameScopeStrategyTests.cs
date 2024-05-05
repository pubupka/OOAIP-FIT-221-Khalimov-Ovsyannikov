using Hwdtech;
using Hwdtech.Ioc;

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
            var gameScopeDict = new Dictionary<string, object>();
            var quantum = 5;
            var gameId = "asdfg";
            var mockCmd = new Mock<ICommand>();
            var uobjectId = 1;
            var uobject = new Mock<IUObject>();
            var uobjectDict = new Dictionary<int, IUObject>() { { uobjectId, uobject.Object } };
            var queueOfCmds = new Queue<ICommand>();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Scope.Dict", (object[] args) => gameScopeDict).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.UObject.Dict", (object[] args) => uobjectDict).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Get.Queue", (object[] args) => queueOfCmds).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.CreateNewScope", (object[] args) => new CreateGameScopeStrategy().Invoke(args)).Execute();

            var gameScope = IoC.Resolve<object>("Game.CreateNewScope", gameId, IoC.Resolve<object>("Scopes.Current"), quantum);

            Assert.Throws<ArgumentException>(() => IoC.Resolve<int>("Game.Get.Time.Quantum"));
            Assert.Throws<ArgumentException>(() => IoC.Resolve<ICommand>("Game.Queue.Inqueue", gameId, mockCmd.Object));
            Assert.Throws<ArgumentException>(() => IoC.Resolve<ICommand>("Game.Queue.Dequeue", gameId));
            Assert.Throws<ArgumentException>(() => IoC.Resolve<IUObject>("Game.Get.UObject", uobjectId));
            Assert.Throws<ArgumentException>(() => IoC.Resolve<ICommand>("Game.Delete.UObject", uobjectId));

            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", gameScope).Execute();

            Assert.Equal(IoC.Resolve<int>("Game.Get.Time.Quantum"), quantum);

            IoC.Resolve<ICommand>("Game.Queue.Inqueue", gameId, mockCmd.Object).Execute();
            Assert.Single(queueOfCmds);
            IoC.Resolve<ICommand>("Game.Queue.Dequeue", gameId).Execute();
            Assert.Empty(queueOfCmds);

            Assert.Equal(IoC.Resolve<IUObject>("Game.Get.UObject", uobjectId), uobject.Object);
            IoC.Resolve<ICommand>("Game.Delete.UObject", uobjectId).Execute();
            Assert.Empty(uobjectDict);
        }
    }
}
