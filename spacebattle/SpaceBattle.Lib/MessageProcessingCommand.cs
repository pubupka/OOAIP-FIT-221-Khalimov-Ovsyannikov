using Hwdtech;

namespace SpaceBattle.Lib
{
    public class MessageProcessingCommand : ICommand
    {
        private readonly IProcessable _message;
        public MessageProcessingCommand(IProcessable message)
        {
            _message = message;
        }

        public void Execute()
        {
            var cmd = IoC.Resolve<ICommand>("Server.ProcessedCommand", _message);
            var id = _message.gameId;

            IoC.Resolve<ICommand>("Server.Game.Queue.PushByID", id, cmd).Execute();
        }
    }
}
