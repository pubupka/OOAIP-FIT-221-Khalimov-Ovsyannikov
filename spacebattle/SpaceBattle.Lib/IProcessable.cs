using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceBattle.Lib
{
    public interface IProcessable
    {
        public string cmdType {get;}
        public string gameId {get;}
        public int gameItemId {get;}
        public IDictionary<string, object> attributes {get;}
    }
}
