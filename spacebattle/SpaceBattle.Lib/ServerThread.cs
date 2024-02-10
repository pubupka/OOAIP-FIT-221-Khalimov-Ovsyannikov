using System.Collections.Concurrent;
using IoC;

namespace SpaceBattle.Lib
{
    class ServerThread 
    {
        private Thread _thread;
        private BlockingCollection<ICommand> _query;
        private bool stop = false;
        Action strategy;

        public ServerThread(BlockingCollection<ICommand> query) 
        {
            _query = query;

            strategy = () => {
                var cmd = query.Take();
                try {
                    cmd.Execute();
                } catch (Exception e) {
                    IoC.Resolve<ICommand>("Game.Exception.Handle", cmd, e).Execute();
                }
            }

            _t = new Thread(() => {
                while(!stop) {
                    strategy(); 
                }
            });
        }

        public void Start() 
        {
            _t.Start();
        }

        internal void Stop() 
        {
            stop = true;
        }

        internal void ChangeStrategy(Action newStrategy) 
        {
            strategy = newStrategy;
        }
    }
}
