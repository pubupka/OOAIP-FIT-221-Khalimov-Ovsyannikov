using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceBattle.Lib
{
    public class GetHashStrategy: IStrategy
    {
        private IEnumerable<Type> _types;
        public GetHashStrategy(IEnumerable<Type> types)
        {
            _types = types;
        }
        public object Invoke(params object[] args)
        {
            var types = _types.OrderBy(x => x.GetHashCode());
            unchecked 
            {
                return _types.Aggregate(1, (total, next) => HashCode.Combine(total, next));
            }
        }
    }
}