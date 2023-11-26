using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceBattle.Lib
{
    public interface IQueue
    {
        void Add(ICommand cmd);
        ICommand Take();
    }
}
