namespace SpaceBattle.Lib
{
    public interface IShootable
    {
        Vector velocity
        {
            get;
        }
        Vector position
        {
            get;
            set;
        }
    }
}
