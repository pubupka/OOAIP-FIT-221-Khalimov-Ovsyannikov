namespace SpaceBattle.Lib
{
    public class InjectCommand : ICommand, IInjectable
    {
        private ICommand _cmd;

        public InjectCommand(ICommand cmd) => _cmd = cmd;

        public void Execute() => _cmd.Execute();

        public void Inject(ICommand obj) => _cmd = obj;
    }
}
