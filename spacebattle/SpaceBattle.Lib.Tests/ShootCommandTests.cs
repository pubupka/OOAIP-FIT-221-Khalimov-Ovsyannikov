using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests
{
    public class ShootCommandTests
    {
        public ShootCommandTests()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        }

        [Fact]
        public void ShootCommandTestsPositive()
        {
            var mockShootable = new Mock<IShootable>();
            mockShootable.SetupGet(x => x.velocity).Returns(new Vector(new int[] { 1, 2 }));
            mockShootable.SetupGet(x => x.position).Returns(new Vector(new int[] { 1, 2 }));

            var mockCmd = new Mock<ICommand>();
            mockCmd.Setup(x => x.Execute()).Verifiable();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.ShootCommand", (object[] args) => new ShootCommand((IShootable)args[0])).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Create.Movable", (object[] args) =>
            {
                var shot = (IShootable)args[0];
                var mockMovable = new Mock<IMovable>();
                var velocity = shot.velocity;
                var position = shot.position;
                mockMovable.SetupGet(x => x.Velocity).Returns(velocity);
                mockMovable.SetupGet(x => x.Position).Returns(position);
                return mockMovable.Object;
            }).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.StartMove", (object[] args) => mockCmd.Object).Execute();

            IoC.Resolve<ICommand>("Game.Command.ShootCommand", mockShootable.Object).Execute();
            mockCmd.Verify(x => x.Execute(), Times.Once());
        }

        [Fact]
        public void CantDeterminePosition()
        {
            var mockShootable = new Mock<IShootable>();
            mockShootable.SetupGet(x => x.velocity).Returns(new Vector(new int[] { 1, 2 }));
            mockShootable.SetupGet(x => x.position).Throws(new Exception());

            var mockCmd = new Mock<ICommand>();
            mockCmd.Setup(x => x.Execute()).Verifiable();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.ShootCommand", (object[] args) => new ShootCommand((IShootable)args[0])).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Create.Movable", (object[] args) =>
            {
                var shot = (IShootable)args[0];
                var mockMovable = new Mock<IMovable>();
                var velocity = shot.velocity;
                var position = shot.position;
                mockMovable.SetupGet(x => x.Velocity).Returns(velocity);
                mockMovable.SetupGet(x => x.Position).Returns(position);
                return mockMovable.Object;
            }).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.StartMove", (object[] args) => mockCmd.Object).Execute();

            Assert.Throws<Exception>(() => IoC.Resolve<ICommand>("Game.Command.ShootCommand", mockShootable.Object).Execute());
        }

        [Fact]
        public void CantDetermineVelocity()
        {
            var mockShootable = new Mock<IShootable>();
            mockShootable.SetupGet(x => x.velocity).Throws(new Exception());
            mockShootable.SetupGet(x => x.position).Returns(new Vector(new int[] { 1, 2 }));

            var mockCmd = new Mock<ICommand>();
            mockCmd.Setup(x => x.Execute()).Verifiable();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.ShootCommand", (object[] args) => new ShootCommand((IShootable)args[0])).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Create.Movable", (object[] args) =>
            {
                var shot = (IShootable)args[0];
                var mockMovable = new Mock<IMovable>();
                var velocity = shot.velocity;
                var position = shot.position;
                mockMovable.SetupGet(x => x.Velocity).Returns(velocity);
                mockMovable.SetupGet(x => x.Position).Returns(position);
                return mockMovable.Object;
            }).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.StartMove", (object[] args) => mockCmd.Object).Execute();

            Assert.Throws<Exception>(() => IoC.Resolve<ICommand>("Game.Command.ShootCommand", mockShootable.Object).Execute());
        }

        [Fact]
        public void CantMoveProjectile()
        {
            var mockShootable = new Mock<IShootable>();
            mockShootable.SetupGet(x => x.velocity).Returns(new Vector(new int[] { 1, 2 }));
            mockShootable.SetupGet(x => x.position).Returns(new Vector(new int[] { 1, 2 }));

            var mockCmd = new Mock<ICommand>();
            mockCmd.Setup(x => x.Execute()).Throws(new Exception());

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.ShootCommand", (object[] args) => new ShootCommand((IShootable)args[0])).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Create.Movable", (object[] args) =>
            {
                var shot = (IShootable)args[0];
                var mockMovable = new Mock<IMovable>();
                var velocity = shot.velocity;
                var position = shot.position;
                mockMovable.SetupGet(x => x.Velocity).Returns(velocity);
                mockMovable.SetupGet(x => x.Position).Returns(position);
                return mockMovable.Object;
            }).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.StartMove", (object[] args) => mockCmd.Object).Execute();

            Assert.Throws<Exception>(() => IoC.Resolve<ICommand>("Game.Command.ShootCommand", mockShootable.Object).Execute());
        }
    }
}
