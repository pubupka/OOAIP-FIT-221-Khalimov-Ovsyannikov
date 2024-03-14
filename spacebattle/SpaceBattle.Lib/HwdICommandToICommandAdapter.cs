namespace SpaceBattle.Lib
{
    public class HwdICommandToICommandAdapter : ICommand
    {
        private readonly Hwdtech.ICommand _cmd;
        public HwdICommandToICommandAdapter(Hwdtech.ICommand cmd)
        {
            _cmd = cmd;
        }
        public void Execute()
        {
            _cmd.Execute();
        }
    }
}
