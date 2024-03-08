using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hwdtech;

namespace SpaceBattle.Lib
{
    public class StopServerCommand:ICommand
    {
        private ICommand _threadToStop;

        public StopServerCommand(ICommand threadToStop)
        {
            _threadToStop = threadToStop;
        }

        public void Execute()
        {
            IoC.Resolve<ICommand>("Server.Thread.Stop", _threadToStop).Execute();
        }
    }
}
