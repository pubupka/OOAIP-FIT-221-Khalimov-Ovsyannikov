using SpaceBattle.Lib;
using Hwdtech;

public class RepeatCommand : ICommand
{
    private readonly ICommand _cmdToRepeat;
    public RepeatCommand(ICommand cmdToRepeat)
    {
        _cmdToRepeat = cmdToRepeat;
    }

    public void Execute()
    {
        IoC.Resolve<IQueue>("Game.Queue").Add(_cmdToRepeat);
    }
}
