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
            IoC.Resolve<ICommand>("Game.Command.Move", _shot.type, _shot.velocity, _shot.position).Execute();
        }
    }
}
