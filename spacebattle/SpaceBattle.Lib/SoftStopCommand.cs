namespace SpaceBattle.Lib
{
    public class SoftStopCommand : ICommand
    {
        private readonly ServerThread _thread;
        private readonly int _threadId;
        private readonly Action _actionAfterStop;

        public SoftStopCommand(ServerThread thread, int threadId, Action actionAfterStop)
        {
            _thread = thread;
            _threadId = threadId;
            _actionAfterStop = actionAfterStop;
        }

        public void Execute()
        {
            if (_thread.Id == _threadId)
                _thread.ChangeStrategy(() => {
                    if (_thread.Query.Count == 0)
                    {
                        _thread.Stop();
                        _actionAfterStop();
                    }
                    else
                    {
                        _thread.BaseStrategy();
                    }
                });
            else
                throw new ThreadStateException();
        }
    }
}
