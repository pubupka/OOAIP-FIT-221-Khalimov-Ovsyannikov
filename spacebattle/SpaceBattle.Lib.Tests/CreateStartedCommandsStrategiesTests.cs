using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests
{
    public class CreateStartedCommandsStrategiesTests
    {
        public CreateStartedCommandsStrategiesTests()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        }

        [Fact]
        public void CreateStartedCommandsStrategiesTestsPositiveCreateAllCommands()
        {
            var mockUObject = new Mock<IUObject>();

            var mockStartable = new Mock<IMoveStartable>();
            var mockStopable = new Mock<IEndable>();
            var mockShootable = new Mock<IShootable>();
            var mockTurnable = new Mock<ITurnable>();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.UObject.Adapter.Create.MoveStartable", (object[] args) => mockStartable.Object).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.UObject.Adapter.Create.Endable", (object[] args) => mockStopable.Object).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.UObject.Adapter.Create.Shootable", (object[] args) => mockShootable.Object).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.UObject.Adapter.Create.Turnable", (object[] args) => mockTurnable.Object).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Create.StartMovementCommand", (object[] args) => new CreateStartMoveCommandStrategy().Invoke(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Create.EndMovementCommand", (object[] args) => new CreateEndMovementCommandStrategy().Invoke(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Create.ShootCommand", (object[] args) => new CreateShootCommandStrategy().Invoke(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Create.TurnCommand", (object[] args) => new CreateTurnCommandStrategy().Invoke(args)).Execute();

            var cmd = IoC.Resolve<ICommand>("Game.Command.Create.StartMovementCommand", mockUObject.Object);
            Assert.Equal(typeof(StartMoveCommand), cmd.GetType());

            cmd = IoC.Resolve<ICommand>("Game.Command.Create.EndMovementCommand", mockUObject.Object);
            Assert.Equal(typeof(EndMovementCommand), cmd.GetType());

            cmd = IoC.Resolve<ICommand>("Game.Command.Create.ShootCommand", mockUObject.Object);
            Assert.Equal(typeof(ShootCommand), cmd.GetType());

            cmd = IoC.Resolve<ICommand>("Game.Command.Create.TurnCommand", mockUObject.Object);
            Assert.Equal(typeof(TurnCommand), cmd.GetType());
        }
    }
}
