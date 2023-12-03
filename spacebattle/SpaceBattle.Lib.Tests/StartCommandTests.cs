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
                var target = (IUObject)args[0];
                var properties = (List<Tuple<string, object>>)args[1];

                properties.ForEach(property => target.SetProperty(property.Item1, property.Item2));
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
