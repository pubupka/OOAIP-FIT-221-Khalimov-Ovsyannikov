using System.Reflection;
using Hwdtech;

namespace SpaceBattle.Lib
{
    public class CreateAdapterStrategy : IStrategy
    {
        public object Invoke(params object[] args)
        {
            var uObject = (IUObject)args[0];
            var targetType = (Type)args[1];

            var adapterAssemblyMap = IoC.Resolve<IDictionary<KeyValuePair<Type, Type>, Assembly>>("Game.Get.DictOfAssemblies");
            var pair = new KeyValuePair<Type, Type>(uObject.GetType(), targetType);
            if (!adapterAssemblyMap.TryGetValue(pair, out _))
            {
                IoC.Resolve<ICommand>("Game.GenerateAdapter", uObject.GetType(), targetType).Execute();
            }

            return IoC.Resolve<object>("Game.FindAdapter", uObject, targetType);
        }
    }
}
