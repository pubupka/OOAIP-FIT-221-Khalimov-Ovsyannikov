using Hwdtech;
namespace SpaceBattle.Lib
{
    public class DeleteGameCommand : ICommand
    {
        private readonly string _gameId;
        public DeleteGameCommand(string gameId)
        {
            _gameId = gameId;
        }
        public void Execute()
        {
            var gameDict = IoC.Resolve<IDictionary<string, IInjectable>>("Game.Dict");
            var emptyCmd = IoC.Resolve<ICommand>("Game.Command.EmptyCommand");
            var key = _gameId;
            gameDict[key].Inject(emptyCmd);
            var gameScopes = IoC.Resolve<IDictionary<string, object>>("Game.Scope.Dict");
            gameScopes.Remove(key);
        }
    }
}
