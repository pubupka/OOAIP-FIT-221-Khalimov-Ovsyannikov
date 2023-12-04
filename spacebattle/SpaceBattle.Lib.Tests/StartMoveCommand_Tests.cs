using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests;

public class StartMoveCommand_Tests
{
    static StartMoveCommand_Tests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<ICommand>(
            "IoC.Register", 
            "Game.IUObject.SetProperty",
            (object[] args) =>
            {
                var order = (IUObject)args[0];
                var property = (KeyValuePair<string, object>)args[1];

                order.SetProperty(property.Key, property.Value);
            }
        ).Execute();

        var LongMoveCommand = new Mock<ICommand>();
        IoC.Resolve<ICommand>(
            "IoC.Register", 
            "Game.Commands.LongMove",
            (object[] args) =>
            {
               return LongMoveCommand;
            }
        ).Execute();

        var queue = new Mock<IQueue>();
        IoC.Resolve<ICommand>(
            "IoC.Register", 
            "Game.Queue",
            (object[] args) =>
            {
               return queue;
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

        var startMoveCommand = new StartMoveCommand(startable.Object);
        startMoveCommand.Execute();

        Assert.Contains("id", orderDict.Keys);
        Assert.Contains("Game.Commands.LongMove", orderDict.Keys);
    }
}
