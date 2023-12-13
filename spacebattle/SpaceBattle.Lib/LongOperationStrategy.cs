using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hwdtech;

namespace SpaceBattle.Lib
{
    public class LongOperationStrategy: IStrategy
    {
        private readonly string _name;
        private readonly IUObject _target;

        public LongOperationStrategy(string name, IUObject target)
        {
            _name = name;
            _target = target;
        }
        public object Invoke(params object[] args)
        {
            ICommand cmd = IoC.Resolve<ICommand>("Game.Command." + _name, _target);

            ICommand repeatCmd = IoC.Resolve<ICommand>("Game.Command.Repeat", cmd);
            ICommand injectCmd = IoC.Resolve<ICommand>("Game.Command.Inject", repeatCmd);


            List<ICommand> listCmd = new List<ICommand>(){injectCmd};

            ICommand macroCmd = IoC.Resolve<ICommand>("Game.Command.Macro", listCmd);

            return macroCmd;
        }
    }
}