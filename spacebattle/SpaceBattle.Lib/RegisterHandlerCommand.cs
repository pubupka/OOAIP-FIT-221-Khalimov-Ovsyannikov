using Hwdtech;

namespace SpaceBattle.Lib
{
    public class RegisterHandlerCommand : ICommand
    {
        private readonly IEnumerable<Type> _typesCollection;
        private readonly IHandler _handler;

        public RegisterHandlerCommand(IEnumerable<Type> typesCollection, IHandler handler)
        {
            _typesCollection = typesCollection;
            _handler = handler;
        }

        public void Execute()
        {
            var hashcode = "";
            _typesCollection.ToList().ForEach(type =>
            {
                hashcode += Convert.ToString(type.GetHashCode());
            });

            var tree = IoC.Resolve<IDictionary<string, IHandler>>("Game.ExceptionHandler.Tree");

            if (tree.TryGetValue(hashcode, out var handler))
            {
                return;
            }

            tree.Add(hashcode, _handler);
        }
    }
}
