using Hwdtech;

namespace SpaceBattle.Lib
{
    public class CreateEndMovementCommandStrategy : IStrategy
    {
        public object Invoke(object[] args)
        {
            var obj = (IUObject)args[0];
            return new EndMovementCommand(IoC.Resolve<IEndable>("Game.UObject.Adapter.Create.Endable", obj));
        }
    }
}
