using Hwdtech;
using Hwdtech.Ioc;

public class MacroCommandTests
{
    public MacroCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
    }

    [Fact]
    public void InitializeMacroCommand_Positive()
    {
        var moveCommand = new Mock<ICommand>();
        moveCommand.Setup(mc => mc.Execute()).Callback(() => {}).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.Move", (object[] args) => {
            return moveCommand.Object;
        }).Execute();

        var checkFuelCommand = new Mock<ICommand>();
        checkFuelCommand.Setup(cfc => cfc.Execute()).Callback(() => {}).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.CheckFuel", (object[] args) => {
            return checkFuelCommand.Object;
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.MacroCommands.MoveWithCheckFuel", (object[] args) => {
            return new string[] { "Game.Commands.Move", "Game.Commands.CheckFuel" };
        }).Execute();

        var macroCommand = new MacroCommand("Game.MacroCommands.MoveWithCheckFuel");
        macroCommand.Execute();

        moveCommand.Verify(mc => mc.Execute(), Times.Once());
        checkFuelCommand.Verify(cfc => cfc.Execute(), Times.Once());
    }

    // Тест 2: Одна или несколько команд выкидывают исключение при выполнении
}
