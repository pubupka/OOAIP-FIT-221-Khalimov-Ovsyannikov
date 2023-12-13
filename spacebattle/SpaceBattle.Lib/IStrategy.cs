using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceBattle.Lib
{
    public interface IStrategy
    {
        public object Invoke(params object[] args);
    }
}
