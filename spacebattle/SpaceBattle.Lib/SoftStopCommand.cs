namespace SpaceBattle.Lib
{
    public class SoftStopCommand : ICommand
    {
        private readonly ServerThread _thread;
        private readonly int _id;

        public SoftStopCommand(ServerThread thread, int id)
        {
            _thread = thread;
            _id = id;
        }

        public void Execute()
        {
            _thread.ActionWithIdCheck(
                _id,
                () => _thread.ChangeStrategy(_thread.SoftStopStrategy)
            );
        }
    }
}
