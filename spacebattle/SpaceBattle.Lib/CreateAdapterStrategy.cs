using System.Reflection;
using Hwdtech;

namespace SpaceBattle.Lib
{
    public class CreateAdapterStrategy : IStrategy
    {
        public object Invoke(params object[] args)
        {
            var uObject = (IUObject)args[0];
            var originType = uObject.GetType();
            var targetType = (Type)args[1];

            var dictOfAssemblies = IoC.Resolve<IDictionary<KeyValuePair<Type, Type>, Assembly>>("Game.Get.DictOfAssemblies");
            var key = new KeyValuePair<Type, Type>(originType, targetType);
            if (!dictOfAssemblies.ContainsKey(key))
            {
                IoC.Resolve<ICommand>("Game.GenerateAdapter", originType, targetType).Execute();
            }

            return IoC.Resolve<object>("Game.FindAdapter", uObject, targetType);
        }
    }
}
