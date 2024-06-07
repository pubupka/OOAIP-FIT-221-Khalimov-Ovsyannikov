using Hwdtech;

namespace SpaceBattle.Lib
{
    public class CreateTurnCommandStrategy : IStrategy
    {
        public object Invoke(object[] args)
        {
            var obj = (IUObject)args[0];
            return new TurnCommand(IoC.Resolve<ITurnable>("Game.UObject.Adapter.Create.Turnable", obj));
        }
    }
}
