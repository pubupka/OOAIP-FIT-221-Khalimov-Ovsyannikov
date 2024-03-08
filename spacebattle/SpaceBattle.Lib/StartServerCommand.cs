using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hwdtech;

namespace SpaceBattle.Lib
{
    public class StartServerStrategy: IStrategy
    { 
        public object Invoke(int numberOfThreads)
        {
            List<ICommand> list = new List<ICommand>();
            for(i=0;i< numberOfThreads;i++)
            {
                list.Add(IoC.Resolve<ICommand>("Server.Thread.Start"));
            }
            return list;
        }
    }
}
