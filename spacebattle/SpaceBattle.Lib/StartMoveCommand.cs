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
        _startable.Properties.ForEach(property => IoC.Resolve<ICommand>(
            "Properties.Set",
            _startable.Target,
            property.Item1,
            property.Item2
        ).Execute());

        var cmd = IoC.Resolve<ICommand>(
            "LongOperations.Move",
            _startable.Target
        );

        IoC.Resolve<ICommand>(
            "Properties.Set",
            _startable.Target,
            "LongOperation.Move",
            cmd
        ).Execute();

        IoC.Resolve<IQueue>(
            "Game.Queue"
        ).Add(cmd);
    }
}
