using SpaceBattle.Lib;
using Hwdtech;

public class FindHandlerCommand : ICommand
{
    private readonly Dictionary<Type, Dictionary<Type, IHandler>> _handleTree;
    private readonly Type _cmdType;
    private readonly Type _exType;

    public FindHandlerCommand(Type cmdType, Type exType)
    {
        _handleTree = IoC.Resolve<Dictionary<Type, Dictionary<Type, IHandler>>>("Game.GetHandleTree");
        _cmdType = cmdType;
        _exType = exType;
    }

    public void Execute()
    {
        var subtree = _handleTree.GetValueOrDefault(_cmdType, new Dictionary<Type, IHandler>() {
            { typeof(Exception), IoC.Resolve<IHandler>("Game.DefaultHandler") }
        });

        var handler = subtree.GetValueOrDefault(_exType, IoC.Resolve<IHandler>("Game.DefaultHandler"));

        IoC.Resolve<ICommand>("Game.HandleExceptionCommand", _cmdType, handler);

        // if (_handleTree.TryGetValue(_cmdName, out var dict) && dict.TryGetValue(_exName, out var handler))
        // {
        //     IoC.Resolve<ICommand>("Game.HandleExceptionCommand", _cmdName, handler).Execute();
        // }
    }
}
