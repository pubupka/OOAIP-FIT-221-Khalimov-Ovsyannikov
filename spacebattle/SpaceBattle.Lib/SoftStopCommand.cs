namespace SpaceBattle.Lib
{
    public class SoftStopCommand : ICommand
    {
        private readonly ServerThread _thread;
        private readonly int _threadId;

        public SoftStopCommand(ServerThread thread, int threadId)
        {
            _thread = thread;
            _threadId = threadId;
        }

        public void Execute()
        {
            if (_thread.GetId() == _threadId)
                _thread.ChangeStrategy(() => {
                    if (_thread.IsEmpty())
                    {
                        _thread.Stop();
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
