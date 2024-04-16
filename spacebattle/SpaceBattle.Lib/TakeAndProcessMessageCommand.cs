using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hwdtech;

namespace SpaceBattle.Lib
{
    public class TakeAndProcessMessageCommand:ICommand
    {
        public void Execute()
        {
            var queueOfMessages = IoC.Resolve<Queue<IProcessable>>("Server.Take.QueueOfMessages");
            new MessageProcessingCommand(queueOfMessages.Dequeue()).Execute();
        }
    }
}