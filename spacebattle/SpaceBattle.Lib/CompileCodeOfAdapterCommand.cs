using System.Reflection;
using Hwdtech;

namespace SpaceBattle.Lib
{
    public class CompileCodeOfAdapterCommand : ICommand
    {
        private readonly Type _primaryType;
        private readonly Type _targetType;

        public CompileCodeOfAdapterCommand(Type primaryType, Type targetType)
        {
            _primaryType = primaryType;
            _targetType = targetType;
        }

        public void Execute()
        {
            var adapterCode = IoC.Resolve<string>("Game.Adapter.Code", _primaryType, _targetType);
            var assembly = IoC.Resolve<Assembly>("Game.Code.Compile", adapterCode);

            IoC.Resolve<ICommand>("Game.Adapter.TypesAssemblyDict.AddNewElement", _primaryType, _targetType, assembly).Execute();
        }
    }
}
