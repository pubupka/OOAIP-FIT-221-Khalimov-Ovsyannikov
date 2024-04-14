namespace SpaceBattle.Lib
{
    public class ActionCommand : ICommand
    {
        private readonly Action _act;
        public ActionCommand(Action act)
        {
            _act = act;
        }
        public void Execute()
        {
            _act();
        }
    }
}
