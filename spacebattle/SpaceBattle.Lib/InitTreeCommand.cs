using Hwdtech;

public class InitTreeCommand : ICommand
{
    private readonly Dictionary<Type, object> _tree;
    public InitTreeCommand(Dictionary<Type, object> tree)
    {
        _tree = tree;
    }
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetHandleTree", (object[] args) => _tree).Execute();
    }
}
