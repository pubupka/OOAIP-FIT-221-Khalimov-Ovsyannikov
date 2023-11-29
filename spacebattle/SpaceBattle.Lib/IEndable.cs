using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceBattle.Lib
{
    public interface IEndable
    {
        public IUObject target { get; }
        public IDictionary<string, int> propertyAndValue { get; }
    }
}
