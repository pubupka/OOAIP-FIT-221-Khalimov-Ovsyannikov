using System.Collections.Concurrent;
using Hwdtech;

namespace SpaceBattle.Lib
{
    public class InitServerThreadDependenciesCommand : ICommand
    {
        public void Execute()
        {
            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Create And Start Thread",
                (object[] args) =>
                {
                    var serverThread = new ServerThread(
                        new BlockingCollection<ICommand>()
                    );
                    serverThread.Start();
                    var act = (Action)args[0];
                    act();
                    return serverThread;
                }
            ).Execute();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Send Command",
                (object[] args) =>
                {
                    var thread = IoC.Resolve<ServerThread>("GetThreadById", (int)args[0]);
                    var cmd = (ICommand)args[1];
                    return new SendCommand(thread, cmd);
                }
            ).Execute();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Hard Stop The Thread",
                (object[] args) =>
                {
                    var thread = IoC.Resolve<ServerThread>("GetThreadById", (int)args[0]);
                    var hardStopCommand = new HardStopCommand(thread);

                    var commandWrapper = new CommandWrapper(hardStopCommand, (Action)args[1]);

                    return commandWrapper;
                }
            ).Execute();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Soft Stop The Thread",
                (object[] args) =>
                {
                    var thread = IoC.Resolve<ServerThread>("GetThreadById", (int)args[0]);
                    var softStopCommand = new SoftStopCommand(thread, (Action)args[1]);

                    return softStopCommand;
                }
            ).Execute();
        }
    }
}
