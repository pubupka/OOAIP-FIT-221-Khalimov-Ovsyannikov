namespace SpaceBattle.Lib
{
    public class HardStopCommand : ICommand
    {
        private readonly ServerThread _thread;
        private readonly int _threadId;

        public HardStopCommand(ServerThread thread, int threadId)
        {
            _thread = thread;
            _threadId = threadId;
        }

        public void Execute()
        {
            // _thread.Stop();
            // _actionAfterStop();

            if (_thread.GetId() == _threadId)
            {
                _thread.Stop();
            }
            else
                throw new ThreadStateException();
        }
    }
}
