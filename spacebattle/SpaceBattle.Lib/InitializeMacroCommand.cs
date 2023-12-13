using Hwdtech;

public class MacroCommand: ICommand
{
    public List<ICommand> _cmds;

    public MacroCommand(string nameOfDependency_returnsAtomaricCmdNames)
    {
        var cmdNames = IoC.Resolve<string[]>(nameOfDependency_returnsAtomaricCmdNames);
        cmdNames.ToList().ForEach(cmd_name =>
        {
            _cmds.Append(IoC.Resolve<ICommand>(cmd_name));
        });
    }

    public void Execute()
    {
        _cmds.ForEach(cmd => cmd.Execute());
    }
}
