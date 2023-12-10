using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hwdtech;

namespace SpaceBattle.Lib
{
    public class LongOperationCommand: ICommand
    {
        private readonly string _name;
        private readonly IUObject _target;

        public LongOperationCommand(string name, IUObject target)
        {
            _name = name;
            _target = target;
        }
        public void Execute()
        {
            ICommand cmd = IoC.Resolve<ICommand>("Game.Command." + _name, _target);

            List<ICommand> listcmd = new List<ICommand>(){cmd};

            ICommand macroCmd = IoC.Resolve<ICommand>("Game.Command.Macro", listcmd);

            macroCmd.Execute();
        }
    }
}