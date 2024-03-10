using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hwdtech;

namespace SpaceBattle.Lib
{
    public class StopServerCommand:ICommand
    {
        private string _threadId;

        public StopServerCommand(string threadId)
        {
            _threadId = threadId;
        }

        public void Execute()
        {
            IoC.Resolve<ICommand>("Server.Thread.SoftStop", _threadId).Execute();
        }
    }
}
