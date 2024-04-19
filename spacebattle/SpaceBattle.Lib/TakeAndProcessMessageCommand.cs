using Hwdtech;

namespace SpaceBattle.Lib
{
    public class TakeAndProcessMessageCommand : ICommand
    {
        public void Execute()
        {
            var message = IoC.Resolve<IProcessable>("Server.Take.Message");
            IoC.Resolve<MessageProcessingCommand>("Game.Commands.MessageProcessingCommand", message).Execute();
        }
    }
}
