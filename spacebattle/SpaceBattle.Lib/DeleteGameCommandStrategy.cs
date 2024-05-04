namespace SpaceBattle.Lib
{
    public class DeleteGameCommandStrategy
    {
        public static object Invoke(params object[] args)
        {
            var gameId = (string)args[0];

            return new DeleteGameCommand(gameId);
        }
    }
}
