using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hwdtech;

namespace SpaceBattle.Lib
{
    public class StartCommand:ICommand
    {
        private ICommand _cmd;
        public StartCommand(ICommand cmd)
        {
            _cmd = cmd;
        }
        public void Execute()
        {
            var injectCmd = IoC.Resolve<InjectCommand>("Game.Command.Inject", _cmd);
            var repeatCmd = IoC.Resolve<ICommand>("Game.Command.Repeat", injectCmd);

            repeatCmd.Execute();
        }
    }
}
