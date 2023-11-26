using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceBattle.Lib
{
    public interface IUObject
    {
        public object GetProperty(string name);
        public void SetProperty(string name, object value);
    }
}
