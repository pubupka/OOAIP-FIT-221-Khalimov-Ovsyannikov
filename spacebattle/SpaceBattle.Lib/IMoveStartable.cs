using SpaceBattle.Lib;

public interface IMoveStartable
{
    public IUObject Target { get; }
    public List<Tuple<string, object>> Properties { get; }
}
