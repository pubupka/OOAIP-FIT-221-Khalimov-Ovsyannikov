using Hwdtech;

namespace SpaceBattle.Lib
{
    public class EndMovementCommand
    {
        private readonly IEndable _endable;

        public EndMovementCommand(IEndable endable) => _endable = endable;

        public void Execute()
        {
            IoC.Resolve<string>("Game.UObject.DeleteProperty", _endable.target, _endable.property);
            var command = _endable.command;
            var emptyCommand = IoC.Resolve<ICommand>("Game.Command.EmptyCommand");
            IoC.Resolve<IInjectable>("Game.Command.Inject", command, emptyCommand);
        }
    }
}
