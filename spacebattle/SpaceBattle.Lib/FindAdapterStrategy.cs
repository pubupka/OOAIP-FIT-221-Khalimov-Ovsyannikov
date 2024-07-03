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

            var assembly = IoC.Resolve<Assembly>("Game.Get.Assembly.DictOfAssemblies", uObject.GetType(), targetType);
            var type = assembly.GetType(IoC.Resolve<string>("Game.Adapter.Name", targetType))!;

            return Activator.CreateInstance(type, uObject)!;
        }
    }
}
