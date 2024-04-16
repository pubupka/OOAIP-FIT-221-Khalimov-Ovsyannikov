using Hwdtech;

namespace SpaceBattle.Lib
{
    public class TakeAndProcessMessageCommand : ICommand
    {
        public void Execute()
        {
            var queueOfMessages = IoC.Resolve<Queue<IProcessable>>("Server.Take.QueueOfMessages");
            new MessageProcessingCommand(queueOfMessages.Dequeue()).Execute();
        }
    }
}
