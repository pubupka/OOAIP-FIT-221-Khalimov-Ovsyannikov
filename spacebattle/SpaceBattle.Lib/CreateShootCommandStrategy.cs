using Hwdtech;

namespace SpaceBattle.Lib
{
    public class CreateShootCommandStrategy : IStrategy
    {
        public object Invoke(object[] args)
        {
            var obj = (IUObject)args[0];
            return new ShootCommand(IoC.Resolve<IShootable>("Game.UObject.Adapter.Create.Shootable", obj));
        }
    }
}
