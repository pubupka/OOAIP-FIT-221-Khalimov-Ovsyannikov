using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests
{
    public class MessageProcessingCommandTests
    {
        public MessageProcessingCommandTests()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Game.Queue.PushByID", (object[] args) =>
            {
                return new PushByIdStrategy().Invoke(args);
            }).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.MessageProcessingCommand", (object[] args) => new MessageProcessingCommand((IProcessable)args[0])).Execute();
        }

        [Fact]
        public void MessageProcessingCommandPositiveTest()
        {
            var mockMessage = new Mock<IProcessable>();
            mockMessage.SetupGet(x => x.gameId).Returns("asdfg");
            mockMessage.SetupGet(x => x.gameItemId).Returns(1);
            mockMessage.SetupGet(x => x.attributes).Returns(new Dictionary<string, object>() { { "initial velocity", 2 } });
            mockMessage.SetupGet(x => x.cmdType).Returns("StartMoveCommand");

            var mockItem = new Mock<IUObject>();
            mockItem.Setup(x => x.SetProperty(It.IsAny<string>(), It.IsAny<object>()));

            var mockCmd = new Mock<ICommand>();
            mockCmd.Setup(x => x.Execute()).Verifiable();

            var itemsByIdDict = new Dictionary<int, IUObject>();
            itemsByIdDict.Add(1, mockItem.Object);

            var gameItemsdict = new Dictionary<string, Dictionary<int, IUObject>>() { { "asdfg", itemsByIdDict } };
            var queueOfCmds = new Queue<ICommand>();
            var gameCmdDict = new Dictionary<string, Queue<ICommand>>() { { "asdfg", queueOfCmds } };

            var queueOfMessages = new Queue<IProcessable>();
            queueOfMessages.Enqueue(mockMessage.Object);

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.StartMoveCommand", (object[] args) =>
            {
                return mockCmd.Object;
            }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GetItemFromGameItemsdict", (object[] args) => gameItemsdict[(string) args[0]][(int) args[1]]).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.ProcessedCommand", (object[] args) =>
            {
                return new SettingPropertiesAndReturningCommandByMessageStrategy().Invoke(args);
            }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Get.Queue", (object[] args) =>
            {
                var id = (string)args[0];
                return gameCmdDict[id];
            }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Take.Message", (object[] args) => queueOfMessages.Dequeue()).Execute();

            new TakeAndProcessMessageCommand().Execute();

            Assert.NotEmpty(queueOfCmds);
            queueOfCmds.Dequeue().Execute();
            mockCmd.Verify();
        }

        [Fact]
        public void MessageProcessingCommandNonExistentItemId()
        {
            var mockMessage = new Mock<IProcessable>();
            mockMessage.SetupGet(x => x.gameId).Returns("asdfg");
            mockMessage.SetupGet(x => x.gameItemId).Returns(1);

            var mockItem = new Mock<IUObject>();
            mockItem.Setup(x => x.SetProperty(It.IsAny<string>(), It.IsAny<object>()));
            var itemsByIdDict = new Dictionary<int, IUObject>();
            itemsByIdDict.Add(2, mockItem.Object);

            var queueOfMessages = new Queue<IProcessable>();
            queueOfMessages.Enqueue(mockMessage.Object);

            var gameItemsdict = new Dictionary<string, Dictionary<int, IUObject>>() { { "asdfg", itemsByIdDict } };

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GetItemFromGameItemsdict", (object[] args) => gameItemsdict[(string) args[0]][(int) args[1]]).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.ProcessedCommand", (object[] args) =>
            {
                return new SettingPropertiesAndReturningCommandByMessageStrategy().Invoke(args);
            }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Take.Message", (object[] args) => queueOfMessages.Dequeue()).Execute();

            Assert.Throws<KeyNotFoundException>(new TakeAndProcessMessageCommand().Execute);
        }

        [Fact]
        public void MessageProcessingCommandNonExistentGameId()
        {
            var mockMessage = new Mock<IProcessable>();
            mockMessage.SetupGet(x => x.gameId).Returns("a");

            var mockItem = new Mock<IUObject>();

            var itemsByIdDict = new Dictionary<int, IUObject>();
            itemsByIdDict.Add(1, mockItem.Object);

            var gameItemsdict = new Dictionary<string, Dictionary<int, IUObject>>() { { "asdfg", itemsByIdDict } };

            var queueOfMessages = new Queue<IProcessable>();
            queueOfMessages.Enqueue(mockMessage.Object);

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GetItemFromGameItemsdict", (object[] args) => gameItemsdict[(string) args[0]][(int) args[1]]).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.ProcessedCommand", (object[] args) =>
            {
                return new SettingPropertiesAndReturningCommandByMessageStrategy().Invoke(args);
            }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Take.Message", (object[] args) => queueOfMessages.Dequeue()).Execute();

            Assert.Throws<KeyNotFoundException>(new TakeAndProcessMessageCommand().Execute);
        }

        [Fact]
        public void MessageProcessingCommandCantWriteAttributes()
        {
            var mockMessage = new Mock<IProcessable>();
            mockMessage.SetupGet(x => x.gameId).Returns("asdfg");
            mockMessage.SetupGet(x => x.gameItemId).Returns(1);
            mockMessage.SetupGet(x => x.attributes).Returns(new Dictionary<string, object>() { { "initial velocity", 2 } });
            mockMessage.SetupGet(x => x.cmdType).Returns("StartMoveCommand");

            var mockItem = new Mock<IUObject>();
            mockItem.Setup(x => x.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Throws(new NullReferenceException());

            var itemsByIdDict = new Dictionary<int, IUObject>();
            itemsByIdDict.Add(1, mockItem.Object);

            var gameItemsdict = new Dictionary<string, Dictionary<int, IUObject>>() { { "asdfg", itemsByIdDict } };

            var queueOfMessages = new Queue<IProcessable>();
            queueOfMessages.Enqueue(mockMessage.Object);

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GetItemFromGameItemsdict", (object[] args) => gameItemsdict[(string) args[0]][(int) args[1]]).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.ProcessedCommand", (object[] args) =>
            {
                return new SettingPropertiesAndReturningCommandByMessageStrategy().Invoke(args);
            }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Take.Message", (object[] args) => queueOfMessages.Dequeue()).Execute();

            Assert.Throws<NullReferenceException>(new TakeAndProcessMessageCommand().Execute);
        }
    }
}
