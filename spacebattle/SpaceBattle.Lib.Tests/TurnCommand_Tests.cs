namespace SpaceBattle.Lib.Tests;

public class TurnCommand_Tests
{
    [Fact]
    public void TurnCommandPositive()
    {
        Mock<ITurnable> turnable = new Mock<ITurnable>();
        turnable.SetupGet(m => m.angle).Returns(new Rational(45)).Verifiable();
        turnable.SetupGet(m => m.delta).Returns(new Rational(45)).Verifiable();

        TurnCommand turnCommand = new TurnCommand(turnable.Object);
        turnCommand.Execute();

        turnable.VerifySet(m => m.angle = new Rational(90), Times.Once);
    }

    [Fact]
    public void CantDetermineAngle()
    {
        Mock<ITurnable> turnable = new Mock<ITurnable>();
        turnable.SetupGet(m => m.angle).Throws(new Exception()).Verifiable();
        turnable.SetupGet(m => m.delta).Returns(new Rational(45)).Verifiable();

        TurnCommand turnCommand = new TurnCommand(turnable.Object);
        Assert.Throws<Exception>(() => turnCommand.Execute());
    }

    [Fact]
    public void CantDetermineDelta()
    {
        Mock<ITurnable> turnable = new Mock<ITurnable>();
        turnable.SetupGet(m => m.angle).Returns(new Rational(45)).Verifiable();
        turnable.SetupGet(m => m.delta).Throws(new Exception()).Verifiable();

        TurnCommand turnCommand = new TurnCommand(turnable.Object);
        Assert.Throws<Exception>(() => turnCommand.Execute());
    }

    [Fact]
    public void CantChangeAngle()
    {
        Mock<ITurnable> turnable = new Mock<ITurnable>();
        turnable.SetupGet(m => m.angle).Returns(new Rational(45)).Verifiable();
        turnable.SetupGet(m => m.delta).Returns(new Rational(45)).Verifiable();
        turnable.SetupSet(m => m.angle = It.IsAny<Rational>()).Throws(new Exception()).Verifiable();
        TurnCommand turnCommand = new TurnCommand(turnable.Object);

        Assert.Throws<Exception>(() => turnCommand.Execute());
    }
}