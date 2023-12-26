using SpaceBattle.Lib;
using Hwdtech;

public class FindHandlerCommand : ICommand
{
    private readonly Dictionary<string, Dictionary<string, IHandler>> _handleTree;
    private readonly string _cmdName;
    private readonly string _exName;

    public FindHandlerCommand(string cmdName, string exName)
    {
        _handleTree = IoC.Resolve<Dictionary<string, Dictionary<string, IHandler>>>("Game.GetHandleTree");
        _cmdName = cmdName;
        _exName = exName;
    }

    public void Execute()
    {
        if (_handleTree.TryGetValue(_cmdName, out var dict) && dict.TryGetValue(_exName, out var handler))
        {
            IoC.Resolve<ICommand>("Game.HandleExceptionCommand", _cmdName, handler).Execute();
        }
    }
}
