using System.Collections.Concurrent;
using IoC;

namespace SpaceBattle.Lib
{
    class ServerThread 
    {
        private Thread _t;
        private BlockingCollection<ICommand> _q;
        private bool stop = false;
        Action strategy;

        public ServerThread(BlockingCollection<ICommand> q) 
        {
            _q = q;

            strategy = () => {
                var cmd = q.Take();
                try {
                    cmd.Execute();
                } catch (Exception e) {
                    IoC.Resolve<ICommand>("Exception.Handle", cmd, e).Execute();
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
