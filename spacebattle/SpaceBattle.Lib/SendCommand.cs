namespace SpaceBattle.Lib
{
    public class SendCommand : ICommand
    {
        private readonly ServerThread _thread;
        private readonly ICommand _cmd;

        public SendCommand(ServerThread thread, ICommand cmd)
        {
            _thread = thread;
            _cmd = cmd;
        }

        public void Execute()
        {
            _thread.AddCommand(_cmd);
        }
    }
}
