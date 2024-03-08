using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceBattle.Lib
{
    public interface IRealHandler
    {
        public void Handle(ICommand cmd, Exception ex);
    }
}