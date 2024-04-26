namespace SpaceBattle.Lib
{
    public interface IProcessable
    {
        public string cmdType { get; }
        public string gameId { get; }
        public int gameItemId { get; }
        public IDictionary<string, object> attributes { get; }
    }
}
