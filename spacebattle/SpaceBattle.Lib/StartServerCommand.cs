using Hwdtech;

namespace SpaceBattle.Lib
{
    public class StartServerCommand : ICommand
    {
        private readonly string _threadId;
        public StartServerCommand(string threadId)
        {
            _threadId = threadId;
        }
        public void Execute()
        {
            var _thread = IoC.Resolve<ICommand>("Server.Thread");
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Threads.Collection." + _threadId, (object[] args) =>
            {
                var thread = _thread;
                return thread;
            }).Execute();
            _thread.Execute();
        }
    }
}
