using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceBattle.Lib
{
    public class EmptyCommand : ICommand
    {
        public void Execute() {}
    }
}
