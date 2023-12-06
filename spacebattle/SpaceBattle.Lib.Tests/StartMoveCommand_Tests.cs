using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests;

public class StartMoveCommand_Tests
{
    private static readonly Mock<IQueue> s_queue = new();
    private static readonly Queue<ICommand> s_realQueue = new();

    static StartMoveCommand_Tests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

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

        var LongMoveCommand = new Mock<ICommand>().Object;
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Commands.LongMove",
            (object[] args) =>
            {
                return LongMoveCommand;
            }
        ).Execute();

        s_queue.Setup(q => q.Add(It.IsAny<ICommand>())).Callback(s_realQueue.Enqueue);
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Queue",
            (object[] args) =>
            {
                return s_queue.Object;
            }
        ).Execute();
    }

    [Fact]
    public void StartMoveCommand_Positive()
    {
        var startable = new Mock<IMoveStartable>();
        var order = new Mock<IUObject>();
        var orderDict = new Dictionary<string, object>();

        var properties = new Dictionary<string, object> {
            // { "velocity", new Vector( new int[] { 1, 2 }) },
            // { "position", new Vector( new int[] { 2, 1 }) },
            { "id", 1 },
            // { "action", new Mock<ICommand>() }
        };

        startable.SetupGet(s => s.PropertiesOfOrder).Returns(properties);
        startable.SetupGet(s => s.Order).Returns(order.Object);
        order.Setup(o => o.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Callback<string, object>(orderDict.Add);
        s_queue.Setup(q => q.Add(It.IsAny<ICommand>())).Callback(s_realQueue.Enqueue);

        var startMoveCommand = new StartMoveCommand(startable.Object);
        startMoveCommand.Execute();

        Assert.Contains("id", orderDict.Keys);
        Assert.Contains("Game.Commands.LongMove", orderDict.Keys);
        Assert.NotEmpty(s_realQueue);
    }

    [Fact]
    public void StartMoveCommand_CantSetProperties()
    {
        var startable = new Mock<IMoveStartable>();
        var order = new Mock<IUObject>();
        var orderDict = new Dictionary<string, object>();

        var properties = new Dictionary<string, object> {
            // { "velocity", new Vector( new int[] { 1, 2 }) },
            { "position", new Vector( new int[] { 2, 1 }) },
            // { "id", 1 },
            // { "action", new Mock<ICommand>() }
        };

        startable.SetupGet(s => s.PropertiesOfOrder).Returns(properties);
        startable.SetupGet(s => s.Order).Returns(order.Object);
        order.Setup(o => o.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Callback(() => throw new NotImplementedException());

        var startMoveCommand = new StartMoveCommand(startable.Object);

        Assert.Throws<NotImplementedException>(startMoveCommand.Execute);
    }

    [Fact]
    public void StartMoveCommand_CantPutCommandInQueue()
    {
        var startable = new Mock<IMoveStartable>();
        var order = new Mock<IUObject>();
        var orderDict = new Dictionary<string, object>();

        s_queue.Setup(q => q.Add(It.IsAny<ICommand>())).Callback(() => { });

        var properties = new Dictionary<string, object> {
            // { "velocity", new Vector( new int[] { 1, 2 }) },
            // { "position", new Vector( new int[] { 2, 1 }) },
            // { "id", 1 },
            { "action", new Mock<ICommand>() }
        };

        startable.SetupGet(s => s.PropertiesOfOrder).Returns(properties);
        startable.SetupGet(s => s.Order).Returns(order.Object);
        order.Setup(o => o.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Callback<string, object>(orderDict.Add);

        var startMoveCommand = new StartMoveCommand(startable.Object);
        startMoveCommand.Execute();

        Assert.Empty(s_realQueue);
    }

    [Fact]
    public void StartMoveCommand_CantReadPropertiesOfOrder_FromStartable()
    {
        var startable = new Mock<IMoveStartable>();
        startable.SetupGet(s => s.PropertiesOfOrder).Throws(new NotImplementedException());
        var startMoveCommand = new StartMoveCommand(startable.Object);

        Assert.Throws<NotImplementedException>(startMoveCommand.Execute);
    }

    [Fact]
    public void StartMoveCommand_CantReadOrder_FromStartable()
    {
        var startable = new Mock<IMoveStartable>();
        startable.SetupGet(s => s.Order).Throws(new NotImplementedException());
        var properties = new Dictionary<string, object> {
            // { "velocity", new Vector( new int[] { 1, 2 }) },
            { "position", new Vector( new int[] { 2, 1 }) },
            // { "id", 1 },
            //{ "action", new Mock<ICommand>() }
        };

        startable.SetupGet(s => s.PropertiesOfOrder).Returns(properties);
        var startMoveCommand = new StartMoveCommand(startable.Object);

        Assert.Throws<NotImplementedException>(startMoveCommand.Execute);
    }
}
