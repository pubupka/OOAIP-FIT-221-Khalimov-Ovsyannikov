using System.Collections.Concurrent;

namespace SpaceBattle.Lib
{
    public class SoftStopCommand : ICommand
    {
        ServerThread _thread;
        BlockingCollection<ICommand> _threadQuery;

        public SoftStopCommand(ServerThread thread, BlockingCollection<ICommand> threadQuery)
        {
            _thread = thread;
            _threadQuery = threadQuery
        }

        public void Execute()
        {
            _threadQuery.Add(new HardStopCommand(_thread))
        }
    }
}
