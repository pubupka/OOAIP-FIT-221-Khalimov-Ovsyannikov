namespace SpaceBattle.Lib
{
    public class CommandWrapper : ICommand
    {
        private readonly ICommand _cmd;
        private readonly Action _action;
        public CommandWrapper(ICommand cmd, Action action)
        {
            _cmd = cmd;
            _action = action;
        }

        public void Execute()
        {
            _cmd.Execute();
            _action();
        }
    }
}
