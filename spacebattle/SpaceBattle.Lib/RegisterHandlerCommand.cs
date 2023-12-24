using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hwdtech;

namespace SpaceBattle.Lib
{
    public class RegisterHandlerCommand: ICommand
    {
        private IEnumerable<Type> _typesCollection;
        private IHandler _handler;

        public RegisterHandlerCommand(IEnumerable<Type> typesCollection, IHandler handler)
        {
            _typesCollection = typesCollection;
            _handler = handler;
        }

        public void Execute()
        {
            string hashcode = "";
            _typesCollection.ToList().ForEach(type => {
                hashcode+=Convert.ToString(type.GetHashCode());
            });

            var tree = IoC.Resolve<IDictionary<string, IHandler>>("Game.ExceptionHandler.Tree");

            if (tree.TryGetValue(hashcode, out IHandler? handler))
            {
                return;
            }
            tree.Add(hashcode, _handler);
        }
    }
}