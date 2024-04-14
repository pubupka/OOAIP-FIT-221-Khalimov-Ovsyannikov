using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests
{
    public class LongOperationTests
    {
        public LongOperationTests()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();

            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
            IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.IUObject.SetProperty",
            (object[] args) =>
            {
                var order = (IUObject)args[0];
                var key = (string)args[1];
                var value = args[2];

                order.SetProperty(key, value);
                return new object();
            }
        ).Execute();
        }

        [Fact]
        public void LongOperationTestPositive()
        {
            var mockCommand = new Mock<ICommand>();
            mockCommand.Setup(x => x.Execute()).Verifiable();

            var name = "Movement";
            var mockUObject = new Mock<IUObject>();

            var queue = new Mock<IQueue>();
            var realQueue = new Queue<ICommand>();
            queue.Setup(q => q.Add(It.IsAny<ICommand>())).Callback(realQueue.Enqueue);
            queue.Setup(q => q.Take()).Returns(() => realQueue.Dequeue());

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Game.Queue",
                (object[] args) =>
                {
                    return queue.Object;
                }
            ).Execute();

            var startable = new Mock<IMoveStartable>();
            var order = new Mock<IUObject>();
            var orderDict = new Dictionary<string, object>();
            var properties = new Dictionary<string, object> {
                { "id", 1 },
            };

            startable.SetupGet(s => s.PropertiesOfOrder).Returns(properties);
            startable.SetupGet(s => s.Order).Returns(order.Object);
            order.Setup(o => o.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Callback<string, object>(orderDict.Add);
            queue.Setup(q => q.Add(It.IsAny<ICommand>())).Callback(realQueue.Enqueue);

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command." + name, (object[] args) => mockCommand.Object).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.ConvertToStartable", (object[] args) =>
            {
                var emptyStartableObject = new Mock<IMoveStartable>();
                emptyStartableObject.Setup(x => x.Order).Returns((IUObject)args[0]);
                emptyStartableObject.Setup(x => x.PropertiesOfOrder).Returns(new Dictionary<string, object>());
                return emptyStartableObject.Object;
            }).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.StartMoveCommand", (object[] args) => new StartMoveCommand((IMoveStartable)args[0])).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Macro", (object[] args) => mockCommand.Object).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.Inject", (object[] args) => new InjectCommand(mockCommand.Object)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Operation." + name, (object[] args) => { return new LongOperationStrategy(name, (IUObject)args[0]).Invoke(); }).Execute();

            IoC.Resolve<ICommand>("Game.Operation." + name, mockUObject.Object).Execute();

            queue.Object.Take().Execute();
            mockCommand.Verify();
        }
        [Fact]
        public void LongOperationTestNegative()
        {
            var mockCommand = new Mock<ICommand>();
            mockCommand.Setup(x => x.Execute());

            var unactiveMockCommand = new Mock<ICommand>();
            unactiveMockCommand.Setup(u => u.Execute()).Verifiable();

            var name = "Movement";
            var mockUObject = new Mock<IUObject>();

            var queue = new Mock<IQueue>();
            var realQueue = new Queue<ICommand>();

            queue.Setup(q => q.Add(It.IsAny<ICommand>())).Callback(realQueue.Enqueue);
            queue.Setup(q => q.Take()).Returns(() => realQueue.Dequeue());

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Game.Queue",
                (object[] args) =>
                {
                    return queue.Object;
                }
            ).Execute();

            var startable = new Mock<IMoveStartable>();
            var order = new Mock<IUObject>();
            var orderDict = new Dictionary<string, object>();
            var properties = new Dictionary<string, object> {
                { "id", 1 },
            };

            startable.SetupGet(s => s.PropertiesOfOrder).Returns(properties);
            startable.SetupGet(s => s.Order).Returns(order.Object);
            order.Setup(o => o.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Callback<string, object>(orderDict.Add);
            queue.Setup(q => q.Add(It.IsAny<ICommand>())).Callback(realQueue.Enqueue);

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command." + name, (object[] args) => mockCommand.Object).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.ConvertToStartable", (object[] args) =>
            {
                var emptyStartableObject = new Mock<IMoveStartable>();
                emptyStartableObject.Setup(x => x.Order).Returns((IUObject)args[0]);
                emptyStartableObject.Setup(x => x.PropertiesOfOrder).Returns(new Dictionary<string, object>());
                return emptyStartableObject.Object;
            }).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.StartMoveCommand", (object[] args) => new StartMoveCommand((IMoveStartable)args[0])).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Macro", (object[] args) => mockCommand.Object).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.Inject", (object[] args) => new InjectCommand(mockCommand.Object)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Operation." + name, (object[] args) => { return new LongOperationStrategy(name, (IUObject)args[0]).Invoke(); }).Execute();

            IoC.Resolve<ICommand>("Game.Operation." + name, mockUObject.Object).Execute();

            queue.Object.Take().Execute();
            unactiveMockCommand.Verify(x => x.Execute(), Times.Never);
        }
    }
}
