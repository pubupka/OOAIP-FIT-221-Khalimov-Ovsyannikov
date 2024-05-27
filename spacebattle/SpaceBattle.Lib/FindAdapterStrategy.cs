using System.Reflection;
using Hwdtech;

namespace SpaceBattle.Lib
{
    public class FindAdapterStrategy : IStrategy
    {
        public object Invoke(params object[] args)
        {
            var uObject = (IUObject)args[0];
            var targetType = (Type)args[1];

            var dictOfAssemblies = IoC.Resolve<IDictionary<KeyValuePair<Type, Type>, Assembly>>("Game.Get.DictOfAssemblies");
            var key = new KeyValuePair<Type, Type>(uObject.GetType(), targetType);

            var assembly = dictOfAssemblies[key];
            var type = assembly.GetType(IoC.Resolve<string>("Game.Adapter.Name", targetType))!;

            return Activator.CreateInstance(type, uObject)!;
        }
    }
}
