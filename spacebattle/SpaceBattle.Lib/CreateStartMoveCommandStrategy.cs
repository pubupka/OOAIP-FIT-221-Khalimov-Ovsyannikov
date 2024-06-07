using Hwdtech;

namespace SpaceBattle.Lib
{
    public class CreateStartMoveCommandStrategy : IStrategy
    {
        public object Invoke(object[] args)
        {
            var obj = (IUObject)args[0];
            return new StartMoveCommand(IoC.Resolve<IMoveStartable>("Game.UObject.Adapter.Create.MoveStartable", obj));
        }
    }
}
