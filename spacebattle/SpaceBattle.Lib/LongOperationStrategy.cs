using Hwdtech;

namespace SpaceBattle.Lib
{
    public class LongOperationStrategy : IStrategy
    {
        private readonly string _name;
        private readonly IUObject _target;

        public LongOperationStrategy(string name, IUObject target)
        {
            _name = name;
            _target = target;
        }
        public object Invoke(params object[] args)
        {
            var cmd = IoC.Resolve<ICommand>("Game.Command." + _name, _target);

            var injectCmd = IoC.Resolve<InjectCommand>("Game.Command.Inject", cmd);
            var repeatCmd = IoC.Resolve<ICommand>("Game.Command.Repeat", injectCmd);

            return repeatCmd;
        }
    }
}
