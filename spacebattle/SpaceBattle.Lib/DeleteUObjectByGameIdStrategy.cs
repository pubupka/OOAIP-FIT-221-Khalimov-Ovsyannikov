using Hwdtech;

namespace SpaceBattle.Lib
{
    public class DeleteUObjectByGameIdStrategy : IStrategy
    {
        public object Invoke(object[] args)
        {
            var objectId = (int)args[0];

            var dictOfUobjects = IoC.Resolve<IDictionary<int, IUObject>>("Game.UObject.Dict");
            return new ActionCommand(() => { dictOfUobjects.Remove(objectId); });
        }
    }
}
