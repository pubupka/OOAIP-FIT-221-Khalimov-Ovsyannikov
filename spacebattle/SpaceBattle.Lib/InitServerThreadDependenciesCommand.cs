using Hwdtech;
using System.Collections.Concurrent;

namespace SpaceBattle.Lib
{
    public class InitServerThreadDependenciesCommand : ICommand
    {
        public void Execute()
        {
            IoC.Resolve<ICommand>(
                "IoC.Register",
                "Create And Start Thread",
                (object threadId) => {
                    return new ServerThread(
                        new BlockingCollection<ICommand>(),
                        (int)threadId
                    );
                }
            ).Execute();

            IoC.Resolve<ICommand>(
                "IoC.Register",
                "Send Command",
                (object id, object cmd) => {
                    var thread = IoC.Resolve<ServerThread>("GetThreadById", (int)id);
                    thread.Queue.Add((ICommand)cmd);
                }
            ).Execute();

            IoC.Resolve<ICommand>(
                "IoC.Register",
                "Hard Stop The Thread",
                (object id, object actionAfterStop) => {
                    var thread = IoC.Resolve<ServerThread>("GetThreadById", (int)id);
                    var hardStopCommand = new HardStopCommand(thread, (int)id, (Action)actionAfterStop);

                    var commandWrapper = new CommandWrapper(hardStopCommand, (Action)actionAfterStop);
                    
                    return commandWrapper;
                }
            ).Execute();

            IoC.Resolve<ICommand>(
                "IoC.Register",
                "Soft Stop The Thread",
                (object id, object actionAfterStop) => {
                    var thread = IoC.Resolve<ServerThread>("GetThreadById", (int)id);
                    var softStopCommand = new SoftStopCommand(thread, (int)id, (Action)actionAfterStop);

                    var commandWrapper = new CommandWrapper(softStopCommand, (Action)actionAfterStop);
                    
                    return commandWrapper;
                }
            ).Execute();
        }
    }
}
