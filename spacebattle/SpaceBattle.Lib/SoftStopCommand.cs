namespace SpaceBattle.Lib
{
    public class SoftStopCommand : ICommand
    {
        private readonly ServerThread _thread;
        private readonly Action _act;

        public SoftStopCommand(ServerThread thread, Action act)
        {
            _thread = thread;
            _act = act;
        }

        public void Execute()
        {
            if (_thread.IsCurrent())
            {
                var strategy = _thread.GetStrategy();
                _thread.ChangeStrategy(() =>
                {
                    if (!_thread.IsEmpty())
                    {
                        strategy();
                    }
                    else
                    {
                        _thread.Stop();
                        _act();
                    }
                });
            }
            else
            {
                throw new ThreadStateException();
            }
        }
    }
}
