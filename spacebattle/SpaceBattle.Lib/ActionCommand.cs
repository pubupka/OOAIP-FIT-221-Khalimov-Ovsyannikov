using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceBattle.Lib
{
    public class ActionCommand:ICommand
    {
        private Action _act;
        public ActionCommand(Action act)
        {
            _act = act;
        }
        public void Execute()
        {
            _act();
        }
    }
}
