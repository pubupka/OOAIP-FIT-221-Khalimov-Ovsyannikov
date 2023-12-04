using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests;

public class StartMoveCommand_Tests
{
    static StartMoveCommand_Tests()
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

        var LongMoveCommand = new Mock<ICommand>().Object;
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register", 
            "Game.Commands.LongMove",
            (object[] args) =>
            {
               return LongMoveCommand;
            }
        ).Execute();
    }

    [Fact]
    public void StartMoveCommand_Positive()
    {
        var startable = new Mock<IMoveStartable>();
        var order = new Mock<IUObject>();
        var orderDict = new Dictionary<string, object>();

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

        var properties = new Dictionary<string, object> {
            // { "velocity", new Vector( new int[] { 1, 2 }) },
            // { "position", new Vector( new int[] { 2, 1 }) },
            { "id", 1 },
            // { "action", new Mock<ICommand>() }
        };

        startable.SetupGet(s => s.PropertiesOfOrder).Returns(properties);
        startable.SetupGet(s => s.Order).Returns(order.Object);
        order.Setup(o => o.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Callback<string, object>(orderDict.Add);

        var startMoveCommand = new StartMoveCommand(startable.Object);
        startMoveCommand.Execute();

        Assert.Contains("id", orderDict.Keys);
        Assert.Contains("Game.Commands.LongMove", orderDict.Keys);
        Assert.NotEmpty(realQueue);
    }
}
