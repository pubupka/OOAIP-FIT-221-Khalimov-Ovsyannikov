using Hwdtech;
using SpaceBattle.Lib;

public class StartMoveCommand : ICommand
{
    private readonly IMoveStartable _startable;
    public StartMoveCommand(IMoveStartable startable)
    {
        _startable = startable;
    }

    public void Execute()
    {
        _startable.PropertiesOfOrder.ToList().ForEach(property => IoC.Resolve<ICommand>(
            "Game.IUObject.SetProperty",
            _startable.Order,
            property
        ).Execute());

        var cmd = IoC.Resolve<ICommand>(
            "Game.Commands.LongMove",
            _startable.Order
        );

        IoC.Resolve<ICommand>(
            "Game.IUObject.SetProperty",
            _startable.Order,
            "Game.Commands.LongMove",
            cmd
        ).Execute();

        IoC.Resolve<IQueue>("Game.Queue").Add(cmd);
    }
}
