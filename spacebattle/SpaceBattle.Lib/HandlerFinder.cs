using SpaceBattle.Lib;
using Hwdtech;

public class HandlerFinder
{
    private readonly Dictionary<Type, object> _handleTree;
    //private readonly Dictionary<Type, Dictionary<Type, IHandler>> _handleTree;

    public HandlerFinder()
    {
        _handleTree = IoC.Resolve<Dictionary<Type, object>>("Game.GetHandleTree");
        //_handleTree = IoC.Resolve<Dictionary<Type, Dictionary<Type, IHandler>>>("Game.GetHandleTree");
    }

    public IHandler Find(Type cmdType, Type exType)
    {
        var subtree = (Dictionary<Type, IHandler>)_handleTree.GetValueOrDefault(
            cmdType,
            _handleTree.GetValueOrDefault(
                typeof(ICommand), 
                new Dictionary<Type, IHandler>() { { typeof(IHandler), IoC.Resolve<IHandler>("Game.DefaultHandler") } }
            )
        );

        var handler = subtree.GetValueOrDefault(
            exType,
            subtree.GetValueOrDefault(
                typeof(Exception),
                subtree.GetValueOrDefault(
                    typeof(IHandler),
                    IoC.Resolve<IHandler>("Game.DefaultHandler")
                )
            )
        );

        return handler;

        // IoC.Resolve<ICommand>("Game.HandleExceptionCommand", _cmdType, handler).Execute();


        // var subtree = _handleTree.GetValueOrDefault(_cmdType, new Dictionary<Type, IHandler>() {
        //     { typeof(Exception), IoC.Resolve<IHandler>("Game.DefaultHandler") }
        // });

        // var handler = subtree.GetValueOrDefault(_exType, IoC.Resolve<IHandler>("Game.DefaultHandler"));

        // IoC.Resolve<ICommand>("Game.HandleExceptionCommand", _cmdType, handler).Execute();


        // first version
        // if (_handleTree.TryGetValue(_cmdName, out var dict) && dict.TryGetValue(_exName, out var handler))
        // {
        //     IoC.Resolve<ICommand>("Game.HandleExceptionCommand", _cmdName, handler).Execute();
        // }
    }
}
