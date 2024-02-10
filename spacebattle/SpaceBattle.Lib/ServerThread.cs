using System.Collections.Concurrent;
using Hwdtech;

namespace SpaceBattle.Lib
{
    public class ServerThread 
    {
        private readonly Thread _thread;
        private readonly BlockingCollection<ICommand> _query;
        private bool _stop = false;
        private Action _strategy;

        public ServerThread(BlockingCollection<ICommand> query) 
        {
            _query = query;

            _strategy = () => {
                var cmd = _query.Take();
                try 
                {
                    cmd.Execute();
                } 
                catch (Exception e) 
                {
                    IoC.Resolve<ICommand>("Game.Exception.Handle", cmd, e).Execute();
                }
            };

            _thread = new Thread(() => {
                while(!_stop) 
                {
                    _strategy(); 
                }
            });
        }

        public void Start() 
        {
            _thread.Start();
        }

        internal void Stop() 
        {
            _stop = true;
        }

        internal void ChangeStrategy(Action newStrategy) 
        {
            _strategy = newStrategy;
        }
    }
}
