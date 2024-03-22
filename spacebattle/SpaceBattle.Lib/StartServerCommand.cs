using Hwdtech;

namespace SpaceBattle.Lib
{
    public class StartServerCommand : ICommand
    {
        private readonly int _serverSize;
        public StartServerCommand(int serverSize)
        {
            _serverSize = serverSize;
        }
        public void Execute()
        {
            for(int i = 0; i < _serverSize; i++)
            {
              IoC.Resolve<ICommand>("Server.Thread.Start", i).Execute();  
            }
        }
    }
}
