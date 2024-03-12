using Hwdtech;

namespace SpaceBattle.Lib
{
    public class InitStartAndStopServerCommand : ICommand
    {
        public void Execute()
        {
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Thread.Start", (object[] args) =>
            {
                var id = Convert.ToString(args[0]);
                return new StartServerCommand(id);
            }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Thread.Stop", (object[] args) =>
            {
                var id = Convert.ToString(args[0]);
                return new StopServerCommand(id);
            }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Start.AllThreads", (object[] args) =>
            {
                var count = (int)args[0];
                for (var i = 0; i < count; i++)
                {
                    IoC.Resolve<ICommand>("Server.Thread.Start", i).Execute();
                }

                return new EmptyCommand();
            }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Stop.AllThreads", (object[] args) =>
            {
                var count = (int)args[0];
                for (var i = 0; i < count; i++)
                {
                    IoC.Resolve<ICommand>("Server.Thread.Stop", i).Execute();
                }

                return new EmptyCommand();
            }).Execute();
        }
    }
}
