using Hwdtech;

namespace SpaceBattle.Lib
{
    public class PushByIdStrategy : IStrategy
    {
        public object Invoke(params object[] args)
        {
            var id = (string)args[0];

            var cmd = (ICommand)args[1];

            var queue = IoC.Resolve<Queue<ICommand>>("Server.Get.Queue", id);

            return new ActionCommand(() => { queue.Enqueue(cmd); });
        }
    }
}
