using Hwdtech;

namespace SpaceBattle.Lib
{
    public class ShootCommand : ICommand
    {
        private readonly IShootable _shot;
        public ShootCommand(IShootable shot)
        {
            _shot = shot;
        }
        public void Execute()
        {
            var projectile = IoC.Resolve<IMovable>("Game.Adapter.Create.Movable", _shot);
            IoC.Resolve<ICommand>("Game.Command.StartMove", projectile).Execute();
        }
    }
}
