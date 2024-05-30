using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib
{
    public class SetPropertiesTests
    {
        public SetPropertiesTests()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        }

        [Fact]
        public void SetPositions_Positive()
        {
            var obj1 = new Mock<IUObject>();
            var obj2 = new Mock<IUObject>();
            var uObjects = new List<IUObject>() { obj1.Object, obj2.Object };
            var setPosCmd = new Mock<ICommand>();
            setPosCmd.Setup(x => x.Execute()).Verifiable();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.IUObject.SetProperty", (object[] args) => setPosCmd.Object).Execute();

            new SetPositionsToSpaceshipsCommand(uObjects, new List<Vector>() {
                new(new int[] { 0, 1} ),
                new(new int[] { 0, 2} )
            }).Execute();

            setPosCmd.Verify(x => x.Execute(), Times.Exactly(2));
        }

        [Fact]
        public void SetPositions_Negative()
        {
            var obj = new Mock<IUObject>();
            var uObjects = new List<IUObject>() { obj.Object };
            var setPosCmd = new Mock<ICommand>();
            setPosCmd.Setup(x => x.Execute()).Throws<NotImplementedException>();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.IUObject.SetProperty", (object[] args) => setPosCmd.Object).Execute();

            var setPositionsCmd = new SetPositionsToSpaceshipsCommand(uObjects, new List<Vector>() { });

            Assert.Throws<NotImplementedException>(setPositionsCmd.Execute);
        }

        [Fact]
        public void SetFuel_Positive()
        {
            var obj1 = new Mock<IUObject>();
            var obj2 = new Mock<IUObject>();
            var uObjects = new List<IUObject>() { obj1.Object, obj2.Object };
            var setFuelCmd = new Mock<ICommand>();
            setFuelCmd.Setup(x => x.Execute()).Verifiable();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.IUObject.SetProperty", (object[] args) => setFuelCmd.Object).Execute();

            new SetFuelToSpaceshipsCommand(uObjects, new List<int>() { 10, 12 }).Execute();

            setFuelCmd.Verify(x => x.Execute(), Times.Exactly(2));
        }

        [Fact]
        public void SetFuel_Negative()
        {
            var obj = new Mock<IUObject>();
            var uObjects = new List<IUObject>() { obj.Object };
            var setFuelCmd = new Mock<ICommand>();
            setFuelCmd.Setup(x => x.Execute()).Throws<NotImplementedException>();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.IUObject.SetProperty", (object[] args) => setFuelCmd.Object).Execute();

            var setFuelsCmd = new SetFuelToSpaceshipsCommand(uObjects, new List<int>() { });

            Assert.Throws<NotImplementedException>(setFuelsCmd.Execute);
        }
    }
}
