using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib
{
    public class RealRegisterHandlerCommand: ICommand
    {
        private readonly IEnumerable<Type> _types;
        private readonly IRealHandler _handler;

        public RealRegisterHandlerCommand(IEnumerable<Type> types, IRealHandler handler)
        {
            _types = types;
            _handler = handler;
        }
        public void Execute()
        {
            var list = new List<object>();
            _types.ToList().ForEach(x => list.Add((object) x));

            var hashcode = IoC.Resolve<object>("Game.HashCode", list.ToArray());

            var tree = IoC.Resolve<IDictionary<object, IRealHandler>>("Game.ExceptionHandler.Tree");

            tree.Add(hashcode, _handler);
        }
    }
}