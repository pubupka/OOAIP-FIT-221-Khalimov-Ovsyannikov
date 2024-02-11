using Hwdtech;

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
            if (_thread.Id == _threadId)
                _thread.ChangeStrategy(() => {
                    var cmd = _thread.Query.Take();
                    try 
                    {
                        cmd.Execute();
                    } 
                    catch (Exception e) 
                    {
                        IoC.Resolve<ICommand>("Game.Exception.Handle", cmd, e).Execute();
                    }

                    if (_thread.Query.Count == 0)
                        _thread.Stop();
                });
            else
                throw new ThreadStateException();
        }
    }
}
