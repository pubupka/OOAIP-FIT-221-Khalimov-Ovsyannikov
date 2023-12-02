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
            "SetProperties",
            (object[] args) =>
            {
                var target = (IUObject)args[0];
                var properties = (List<Tuple<string, object>>)args[1];

                properties.ForEach(property => target.SetProperty(property.Item1, property.Item2));
            }
        ).Execute();

        var MoveCommand = new Mock<MoveCommand>();
        IoC.Resolve<ICommand>(
            "IoC.Register", 
            "Game.Commands.Move",
            (object[] args) =>
            {
               return MoveCommand;
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
