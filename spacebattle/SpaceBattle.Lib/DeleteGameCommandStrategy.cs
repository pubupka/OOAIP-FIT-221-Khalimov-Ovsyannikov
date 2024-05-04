namespace SpaceBattle.Lib
{
    public class DeleteGameCommandStrategy : IStrategy
    {
        public object Invoke(params object[] args)
        {
            var gameId = (string)args[0];

            return new DeleteGameCommand(gameId);
        }
    }
}
