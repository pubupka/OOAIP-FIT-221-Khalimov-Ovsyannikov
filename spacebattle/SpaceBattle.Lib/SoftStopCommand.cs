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

            // _thread.ActionWithIdCheck(
            //     _id,
            //     () => _thread.ChangeStrategy(_thread.SoftStopStrategy)
            // );
        }
    }
}

// private ServerThread _t;
// private Action _a;
// public SoftStopCommand(ServerThread t, Action a)
// {
//     _t = t;
//     _a = a;
// }

// public void Execute()
// {
//     if (_t.IsCurrent())
//     {
//         var obehaviour = _t.GetBehaviour();
//         _t.SetBehaviour(() => {
//             if(_t.QCount() != 0)
//             {
//                 obehaviour();
//             }
//             else
//             {
//                 _t.Stop();
//                 _a();
//             }
//         });
//     }
//     else
//     {
//         throw new Exception("ExceptionSoftStop");
//     }
// }
