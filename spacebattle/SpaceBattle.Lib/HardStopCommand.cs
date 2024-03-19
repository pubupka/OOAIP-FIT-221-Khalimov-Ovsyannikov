namespace SpaceBattle.Lib
{
    public class HardStopCommand : ICommand
    {
        private readonly ServerThread _thread;

        public HardStopCommand(ServerThread thread)
        {
            _thread = thread;
        }

        public void Execute()
        {
            if (_thread.IsCurrent())
            {
                _thread.Stop();
            }
            else
            {
                throw new ThreadStateException();
            }
        }
    }
}
