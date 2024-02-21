namespace SpaceBattle.Lib
{
    public class HardStopCommand : ICommand
    {
        private readonly ServerThread _thread;
        private readonly int _threadId;
        private readonly Action _actionAfterStop;

        public HardStopCommand(ServerThread thread, int threadId, Action actionAfterStop)
        {
            _thread = thread;
            _threadId = threadId;
            _actionAfterStop = actionAfterStop;
        }

        public void Execute()
        {
            if (_thread.Id == _threadId)
            {
                _thread.Stop();
                _actionAfterStop();
            }
            else
                throw new ThreadStateException();
        }
    }
}
