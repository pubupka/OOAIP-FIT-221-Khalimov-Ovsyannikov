using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib
{
    public class Program
    {
        public void Main(string[] args)
        {
            var count = int.Parse(args[0]);
            var maincmd = RunMainCommand(count);
            maincmd.Execute();
        }
    }
}
