using Hwdtech;

namespace SpaceBattle.Lib
{
    public class StopServerCommand : ICommand
    {
        private readonly string? _threadId;

        public StopServerCommand(string? threadId)
        {
            _threadId = threadId;
        }

        public void Execute()
        {
            if(_threadId is not ""){
                IoC.Resolve<ICommand>("Server.Thread.SoftStop", _threadId).Execute();
            }
            else throw new Exception();
        }
    }
}
