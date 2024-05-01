using System.Diagnostics;
using Hwdtech;

namespace SpaceBattle.Lib
{
    public class GameCommand : ICommand
    {
        private readonly Queue<ICommand> _q;
        private readonly object _scope;
        private readonly Stopwatch _sw;
        public GameCommand(Queue<ICommand> q, object scope)
        {
            _q = q;
            _scope = scope;
            _sw = new Stopwatch();
        }

        public void Execute()
        {
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", _scope).Execute();

            while (_sw.ElapsedMilliseconds < (int)IoC.Resolve<object>("Game.GetQuant"))
            {
                if (_q.Count == 0)
                {
                    break;
                }

                _sw.Start();
                var cmd = _q.Dequeue();

                try
                {
                    cmd.Execute();
                }
                catch (Exception e)
                {
                    IoC.Resolve<ICommand>("Game.Exception.Handle", cmd, e).Execute();
                }

                _sw.Stop();
            }
        }
    }
}
