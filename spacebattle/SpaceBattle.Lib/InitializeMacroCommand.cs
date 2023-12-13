using Hwdtech;

public class InitializeMacroCommand: ICommand
{
    public List<ICommand> _cmds;

    public InitializeMacroCommand(string cmd_names_dependency_name)
    {
        var cmd_names = IoC.Resolve<string[]>(cmd_names_dependency_name);
        cmd_names.ToList().ForEach(cmd_name =>
        {
            _cmds.Append(IoC.Resolve<ICommand>(cmd_name));
        });
    }

    public void Execute()
    {
        _cmds.ForEach(cmd => cmd.Execute());
    }
}
