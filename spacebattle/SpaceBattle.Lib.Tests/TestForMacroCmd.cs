using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests
{
    public class TestForMacroCmd
    {
        public TestForMacroCmd()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        }

        [Fact]
        public void TestForMacroCmdPositive()
        {
            var cmd = new Mock<ICommand>();
            cmd.Setup(x => x.Execute()).Verifiable();

            new MacroComand(new List<ICommand>() { cmd.Object }).Execute();
            cmd.Verify(x => x.Execute(), Times.Once());
        }
    }
}
