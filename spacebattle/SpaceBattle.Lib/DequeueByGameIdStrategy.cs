using Hwdtech;

namespace SpaceBattle.Lib
{
    public class DequeueByGameIdStrategy : IStrategy
    {
        public object Invoke(object[] args)
        {
            var id = (string)args[0];
            var queue = IoC.Resolve<Queue<ICommand>>("Server.Get.Queue", id);

            return new ActionCommand(() => { queue.Dequeue(); });
        }
    }
}
