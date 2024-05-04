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

        }
    }
}
