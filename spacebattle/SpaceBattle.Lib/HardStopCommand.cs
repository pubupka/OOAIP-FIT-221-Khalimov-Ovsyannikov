namespace SpaceBattle.Lib
{
    public class HardStopCommand : ICommand
    {
        private readonly ServerThread _thread;
        private readonly int _id;

        public HardStopCommand(ServerThread thread, int id)
        {
            _thread = thread;
            _id = id;
        }

        public void Execute()
        {
            _thread.ActionWithIdCheck(_id, _thread.Stop);
        }
    }
}
