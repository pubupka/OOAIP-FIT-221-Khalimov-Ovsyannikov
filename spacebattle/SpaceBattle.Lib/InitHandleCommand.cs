using Hwdtech;

namespace SpaceBattle.Lib
{
    public class InitHandleCommand : ICommand
    {
        public void Execute()
        {
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Exception.Handle", (object[] args) =>
            {
                return new ActionCommand(() =>
                {
                    var cmd = (ICommand)args[0];
                    var e = (Exception)args[1];
                    var handleTree = IoC.Resolve<Dictionary<Type, object>>("Game.GetHandleTree");
                    if (handleTree.ContainsKey(cmd.GetType()))
                    {
                        IoC.Resolve<IHandler>("FindHandler", cmd, e).Handle();
                    }
                    else
                    {
                        e.Data["ThrownByCommand"] = cmd;
                        throw e;
                    }
                });

            }).Execute();
        }
    }
}
