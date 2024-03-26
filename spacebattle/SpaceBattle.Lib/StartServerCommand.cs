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
            
            var indexes = Enumerable.Range(1, _serverSize).ToList();
            indexes.ForEach(i => IoC.Resolve<ICommand>("Server.Thread.Start", i).Execute());  
        }
    }
}
