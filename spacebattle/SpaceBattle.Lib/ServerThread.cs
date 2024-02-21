using System.Collections.Concurrent;
using Hwdtech;

namespace SpaceBattle.Lib
{
    public class ServerThread 
    {
        private readonly BlockingCollection<ICommand> _queue;
        private readonly Thread _thread;
        private bool _stop = false;
        private Action _strategy;

        public ServerThread(BlockingCollection<ICommand> queue) 
        {
            _queue = queue;

            _strategy = BaseStrategy;

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

        public void ChangeStrategy(Action newStrategy) 
        {
            _strategy = newStrategy;
        }

        public void AddCommand(ICommand cmd)
        {
            _queue.Add(cmd);
        }

        public void ActionWithIdCheck(int id, Action action)
        {
            if (id == _thread.ManagedThreadId)
            {
                action();
            }
            else
                throw new ThreadStateException();
        }

        public int GetId()
        {
            return _thread.ManagedThreadId;
        }

        public bool IsEmpty()
        {
            return _queue.Count == 0;
        }

        internal void Stop() 
        {
            _stop = true;
        }

        internal void BaseStrategy()
        {
            var cmd = _queue.Take();
            try 
            {
                cmd.Execute();
            } 
            catch (Exception e) 
            {
                IoC.Resolve<ICommand>("Game.Exception.Handle", cmd, e).Execute();
            }
        }

        internal void SoftStopStrategy()
        {
            if (IsEmpty())
            {
                Stop();
            }
            else
            {
                BaseStrategy();
            }
        }
    }
}
