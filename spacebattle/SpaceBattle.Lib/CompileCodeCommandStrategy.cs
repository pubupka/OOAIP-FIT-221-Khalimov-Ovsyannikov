namespace SpaceBattle.Lib
{
    public class CompileCodeCommandStrategy : IStrategy
    {
        public object Invoke(params object[] args)
        {
            var objectType = (Type)args[0];
            var targetType = (Type)args[1];

            return new CompileCodeOfAdapterCommand(objectType, targetType);
        }
    }
}
