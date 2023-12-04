using Hwdtech;
using Hwdtech.Ioc;
using Xunit;

namespace SpaceBattle.Lib.Tests;

public class StartCommandTests
{
    static StartCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>(
            "IoC.Register", 
            "Game.IUObject.SetProperties",
            (object[] args) =>
            {
                var order = (IUObject)args[0];
                var propertiesOfOrder = (Dictionary<string, object>)args[1];

                propertiesOfOrder.ToList().ForEach(property => order.SetProperty(property.Key, property.Value));
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

    // [Fact]
    // public void ()
    // {

    // }
}
