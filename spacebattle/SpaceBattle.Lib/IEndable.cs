namespace SpaceBattle.Lib
{
    public interface IEndable
    {
        public InjectCommand command { get; }
        public IUObject target { get; }
        public IEnumerable<string> property { get; }
    }
}
