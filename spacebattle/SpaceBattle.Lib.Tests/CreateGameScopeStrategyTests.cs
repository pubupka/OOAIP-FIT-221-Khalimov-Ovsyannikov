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
            Assert.Throws<ArgumentException>(() => IoC.Resolve<ICommand>("Game.Get.UObject", uobjectId));
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

        [Fact]
        public void CreateGameScopeStrategyGameAlreadyHaveItsScope()
        {
            var gameScopeDict = new Dictionary<string, object>() { { "asdfg", (object)0 } };
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

            Assert.Throws<ArgumentException>(() => IoC.Resolve<object>("Game.CreateNewScope", gameId, IoC.Resolve<object>("Scopes.Current"), quantum));
        }

        [Fact]
        public void CreateGameScopeStrategyCreatedGameCommandCantBeExecuted()
        {
            var gameScopeDict = new Dictionary<string, object>();
            var quantum = 5;
            var gameId = "asdfg";
            var mockCmd = new Mock<ICommand>();
            mockCmd.Setup(x => x.Execute()).Throws(new Exception());
            var uobjectId = 1;
            var uobject = new Mock<IUObject>();
            var uobjectDict = new Dictionary<int, IUObject>() { { uobjectId, uobject.Object } };
            var queueOfCmds = new Queue<ICommand>();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Scope.Dict", (object[] args) => gameScopeDict).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.UObject.Dict", (object[] args) => uobjectDict).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Get.Queue", (object[] args) => queueOfCmds).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.CreateNewScope", (object[] args) => new CreateGameScopeStrategy().Invoke(args)).Execute();

            var gameScope = IoC.Resolve<object>("Game.CreateNewScope", gameId, IoC.Resolve<object>("Scopes.Current"), quantum);

            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", gameScope).Execute();

            IoC.Resolve<ICommand>("Game.Queue.Inqueue", gameId, mockCmd.Object).Execute();

            Assert.Throws<Exception>(() => queueOfCmds.Peek().Execute());
        }

        [Fact]
        public void CreateGameScopeStrategyPushToNonExistentQueue()
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

            var dictOfQueues = new Dictionary<string, Queue<ICommand>>();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Get.Queue", (object[] args) => dictOfQueues[(string)args[0]]).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.CreateNewScope", (object[] args) => new CreateGameScopeStrategy().Invoke(args)).Execute();

            var gameScope = IoC.Resolve<object>("Game.CreateNewScope", gameId, IoC.Resolve<object>("Scopes.Current"), quantum);

            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", gameScope).Execute();

            Assert.Throws<KeyNotFoundException>(() => IoC.Resolve<ICommand>("Game.Queue.Inqueue", gameId, mockCmd.Object).Execute());
        }

        [Fact]
        public void CreateGameScopeStrategyTakeFromNonExistentQueue()
        {
            var gameScopeDict = new Dictionary<string, object>();
            var quantum = 5;
            var gameId = "asdfg";
            var mockCmd = new Mock<ICommand>();
            var uobject = new Mock<IUObject>();
            var uobjectDict = new Dictionary<int, IUObject>();
            var queueOfCmds = new Queue<ICommand>();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Scope.Dict", (object[] args) => gameScopeDict).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.UObject.Dict", (object[] args) => uobjectDict).Execute();

            var dictOfQueues = new Dictionary<string, Queue<ICommand>>();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Get.Queue", (object[] args) => dictOfQueues[(string)args[0]]).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.CreateNewScope", (object[] args) => new CreateGameScopeStrategy().Invoke(args)).Execute();

            var gameScope = IoC.Resolve<object>("Game.CreateNewScope", gameId, IoC.Resolve<object>("Scopes.Current"), quantum);

            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", gameScope).Execute();

            Assert.Throws<KeyNotFoundException>(() => IoC.Resolve<ICommand>("Game.Queue.Dequeue", gameId, mockCmd.Object).Execute());
        }

        [Fact]
        public void CreateGameScopeStrategyAddUObjectByWrongId()
        {
            var gameScopeDict = new Dictionary<string, object>();
            var quantum = 5;
            var uobjectId = 1;
            var gameId = "asdfg";
            var mockCmd = new Mock<ICommand>();
            var uobject = new Mock<IUObject>();
            var uobjectDict = new Dictionary<int, IUObject>();
            var queueOfCmds = new Queue<ICommand>();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Scope.Dict", (object[] args) => gameScopeDict).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.UObject.Dict", (object[] args) => uobjectDict).Execute();

            var dictOfQueues = new Dictionary<string, Queue<ICommand>>();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Get.Queue", (object[] args) => dictOfQueues[(string)args[0]]).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.CreateNewScope", (object[] args) => new CreateGameScopeStrategy().Invoke(args)).Execute();

            var gameScope = IoC.Resolve<object>("Game.CreateNewScope", gameId, IoC.Resolve<object>("Scopes.Current"), quantum);

            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", gameScope).Execute();

            Assert.Throws<KeyNotFoundException>(() => IoC.Resolve<ICommand>("Game.Get.UObject", uobjectId));
        }
    }
}
