using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceBattle.Lib
{
    public class DequeueByGameIdStrategy : IStrategy
    {
        public object Invoke(object[] args)
        {
            var id = (string)args[0];
            var queue = IoC.Resolve<Queue<ICommand>>("Server.Get.Queue", id);

            return new ActionCommand(() => { queue.Dequeue(); });
        }
    }
}
