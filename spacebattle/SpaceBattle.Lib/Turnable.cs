namespace SpaceBattle
{
    public interface ITurnable
    {
        public Rational angle { get; set; }
        public Rational delta { get; }
    }

    public class TurnCommand : ICommand
    {
        private readonly ITurnable turnable;
        public TurnCommand(ITurnable turnable)
        {
            this.turnable = turnable;
        }
        public void Execute()
        {
            turnable.angle += turnable.delta;
        }
    }
}
