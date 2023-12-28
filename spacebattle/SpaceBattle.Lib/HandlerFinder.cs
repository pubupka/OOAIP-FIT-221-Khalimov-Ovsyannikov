using SpaceBattle.Lib;
using Hwdtech;

public class HandlerFinder
{
    private readonly Dictionary<Type, object> _handleTree;

    public HandlerFinder()
    {
        _handleTree = IoC.Resolve<Dictionary<Type, object>>("Game.GetHandleTree");
    }

    public IHandler Find(ICommand cmd, Exception ex)
    {
        var cmdType = cmd.GetType();
        var exType = ex.GetType();

        var subtree = (Dictionary<Type, string>)_handleTree.GetValueOrDefault(
            cmdType,
            _handleTree.GetValueOrDefault(
                typeof(ICommand), 
                new Dictionary<Type, string>() { { typeof(string), "Game.DefaultHandler" } }
            )
        );

        var handler_creation_dependency_name = subtree.GetValueOrDefault(
            exType,
            subtree.GetValueOrDefault(
                typeof(Exception),
                subtree.GetValueOrDefault(
                    typeof(string),
                    "Game.DefaultHandler"
                )
            )
        );

        return IoC.Resolve<IHandler>(handler_creation_dependency_name, cmd, ex);
    }
}
