using System.Collections.Concurrent;
using Hwdtech;

namespace SpaceBattle.Lib
{
    public class ServerThread 
    {
        public BlockingCollection<ICommand> Query { get; }
        public int Id { get;}
        private readonly Thread _thread;
        private bool _stop = false;
        private Action _strategy;

        public ServerThread(BlockingCollection<ICommand> query, int id) 
        {
            Query = query;
            Id = id;

            _strategy = () => {
                BaseStrategy();
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

        internal void BaseStrategy()
        {
            var cmd = Query.Take();
            try 
            {
                cmd.Execute();
            } 
            catch (Exception e) 
            {
                IoC.Resolve<ICommand>("Game.Exception.Handle", cmd, e).Execute();
            }
        }
    }
}
