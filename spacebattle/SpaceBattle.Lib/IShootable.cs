namespace SpaceBattle.Lib
{
    public interface IShootable
    {
        string type
        {
            get;
            set;
        }

        Vector velocity
        {
            get;
            set;
        }
        Vector position
        {
            get;
            set;
        }
    }
}
