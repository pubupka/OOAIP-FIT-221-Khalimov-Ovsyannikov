using System.Reflection;
using Hwdtech;

namespace SpaceBattle.Lib
{
    public class CompileCodeOfAdapterCommand : ICommand
    {
        private readonly Type _objectType;
        private readonly Type _targetType;

        public CompileCodeOfAdapterCommand(Type objectType, Type targetType)
        {
            _objectType = objectType;
            _targetType = targetType;
        }

        public void Execute()
        {
            var adapterCode = IoC.Resolve<string>("Game.Adapter.Code", _objectType, _targetType);
            var assembly = IoC.Resolve<Assembly>("Game.Code.Compile", adapterCode);

            IoC.Resolve<ICommand>("Game.Adapter.TypesAssemblyDict.AddNewElement", _objectType, _targetType, assembly).Execute();
        }
    }
}
