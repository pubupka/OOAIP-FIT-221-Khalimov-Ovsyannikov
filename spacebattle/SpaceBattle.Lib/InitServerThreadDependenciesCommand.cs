using Hwdtech;
using System.Collections.Concurrent;

namespace SpaceBattle.Lib
{
    public class InitServerThreadDependenciesCommand : ICommand
    {
        public void Execute()
        {
            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Create And Start Thread",
                (object[] args) => {
                    var serverThread = new ServerThread(
                        new BlockingCollection<ICommand>()
                    );
                    serverThread.Start();
                    return serverThread;
                }
            ).Execute();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Send Command",
                (object[] args) => {
                    var thread = IoC.Resolve<ServerThread>("GetThreadById", (int)args[0]);
                    thread.AddCommand((ICommand)args[1]);
                    return new object();
                }
            ).Execute();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Hard Stop The Thread",
                (object[] args) => {
                    var thread = IoC.Resolve<ServerThread>("GetThreadById", (int)args[0]);
                    var hardStopCommand = new HardStopCommand(thread, (int)args[0]);

                    var commandWrapper = new CommandWrapper(hardStopCommand, (Action)args[1]);
                    
                    return commandWrapper;
                }
            ).Execute();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Soft Stop The Thread",
                (object[] args) => {
                    var thread = IoC.Resolve<ServerThread>("GetThreadById", (int)args[0]);
                    var softStopCommand = new SoftStopCommand(thread, (int)args[0]);

                    var commandWrapper = new CommandWrapper(softStopCommand, (Action)args[1]);
                    
                    return commandWrapper;
                }
            ).Execute();
        }
    }
}
