using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceBattle.Lib
{
    public class DeleteUObjectByGameIdStrategy : IStrategy
    {
        public object Invoke(object[] args)
        {
            var objectId = (int)args[0];

            var dictOfUobjects = IoC.Resolve<IDictionary<int, IUObject>>("Game.UObject.Dict")[objectId];
            return new ActionCommand(()=>{dictOfUobjects.Remove(objectId)});
        }
    }
}
