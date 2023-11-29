using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib
{
    public class EndMovementCommand
    {
        private IEndable _endable;

        public EndMovementCommand(IEndable endable) => _endable = endable;

        public void Execute()
        {
            _endable.propertyAndValue.ToList().ForEach(pair =>IoC.Resolve<ICommand>("Game.UObject.DeleteProperty", _endable.target, pair.Key, pair.Value).Execute());
            IoC.Resolve<IQueue>("Queue.DeleteAllCommands").Take();
            IoC.Resolve<IInjectable>("Game.UObject.GetProperty", _endable.target, "Movement").Inject(IoC.Resolve<ICommand>("Game.Command.EmptyCommand"));
            var emptyCommand = IoC.Resolve<ICommand>("Command.Empty", new EmptyCommand());
            IoC.Resolve<IQueue>("Queue.DeleteAllCommands").Add(emptyCommand);
        }
    }
}
