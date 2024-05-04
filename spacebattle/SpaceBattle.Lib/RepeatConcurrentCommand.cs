using System.Collections.Concurrent;
using Hwdtech;

namespace SpaceBattle.Lib
{
    public class RepeatConcurrentCommand : ICommand
    {
        private readonly ICommand _cmd;

        public RepeatConcurrentCommand(ICommand cmd)
        {
            _cmd = cmd;
        }

        public void Execute()
        {
            IoC.Resolve<BlockingCollection<ICommand>>("Server.Thread.Games").Add(_cmd);
        }
    }
}
