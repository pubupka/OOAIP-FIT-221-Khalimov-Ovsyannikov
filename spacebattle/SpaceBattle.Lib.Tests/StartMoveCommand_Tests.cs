﻿using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests;

public class StartMoveCommand_Tests
{
    public StartMoveCommand_Tests()
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

        var longMoveCommand = new Mock<ICommand>().Object;
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Commands.LongMove",
            (object[] args) =>
            {
                return longMoveCommand;
            }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Commands.Inject",
            (object[] args) =>
            {
                return new InjectCommand((ICommand)args[0]);
            }
        ).Execute();
    }

    [Fact]
    public void StartMoveCommand_Positive()
    {
        var queue = new Mock<IQueue>();
        var realQueue = new Queue<ICommand>();
        queue.Setup(q => q.Add(It.IsAny<ICommand>())).Callback(realQueue.Enqueue);
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

        var startMoveCommand = new StartMoveCommand(startable.Object);
        startMoveCommand.Execute();

        Assert.Contains("id", orderDict.Keys);
        Assert.Contains("Game.Commands.Inject.LongMove", orderDict.Keys);
        Assert.NotEmpty(realQueue);
    }

    [Fact]
    public void StartMoveCommand_CantSetProperties()
    {
        var startable = new Mock<IMoveStartable>();
        var order = new Mock<IUObject>();
        var orderDict = new Dictionary<string, object>();

        var properties = new Dictionary<string, object> {
            { "position", new Vector( new int[] { 2, 1 }) },
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
        var queue = new Mock<IQueue>();
        var realQueue = new Queue<ICommand>();
        queue.Setup(q => q.Add(It.IsAny<ICommand>())).Callback(() => { });
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
            { "action", new Mock<ICommand>() }
        };

        startable.SetupGet(s => s.PropertiesOfOrder).Returns(properties);
        startable.SetupGet(s => s.Order).Returns(order.Object);
        order.Setup(o => o.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Callback<string, object>(orderDict.Add);

        var startMoveCommand = new StartMoveCommand(startable.Object);
        startMoveCommand.Execute();

        Assert.Empty(realQueue);
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
            { "position", new Vector( new int[] { 2, 1 }) },
        };

        startable.SetupGet(s => s.PropertiesOfOrder).Returns(properties);
        var startMoveCommand = new StartMoveCommand(startable.Object);

        Assert.Throws<NotImplementedException>(startMoveCommand.Execute);
    }
}
