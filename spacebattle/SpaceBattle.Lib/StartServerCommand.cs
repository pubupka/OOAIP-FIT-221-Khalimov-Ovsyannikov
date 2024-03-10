using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib
{
    public class StartServerCommand:ICommand
    { 
        private string _threadId;
        public StartServerCommand(string threadId)
        {
            _threadId = threadId;
        }
        public void Execute()
        {
            var _thread = IoC.Resolve<ICommand>("Server.Thread");
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Threads.Collection."+_threadId, (object[] args)=>{
                ICommand thread = _thread;
                return thread;
            }).Execute();
            _thread.Execute();
        }
    }
}
