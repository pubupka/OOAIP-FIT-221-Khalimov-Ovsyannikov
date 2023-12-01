namespace SpaceBattle.Lib.Tests;

public class TurnCommand_Tests
{
    [Fact]
    public void TurnCommandPositive()
    {
        var turnable = new Mock<ITurnable>();
        turnable.SetupGet(m => m.angle).Returns(new Rational(45)).Verifiable();
        turnable.SetupGet(m => m.delta).Returns(new Rational(45)).Verifiable();

        var turnCommand = new TurnCommand(turnable.Object);
        turnCommand.Execute();

        turnable.VerifySet(m => m.angle = new Rational(90), Times.Once);
    }

    [Fact]
    public void CantDetermineAngle()
    {
        var turnable = new Mock<ITurnable>();
        turnable.SetupGet(m => m.angle).Throws(new Exception()).Verifiable();
        turnable.SetupGet(m => m.delta).Returns(new Rational(45)).Verifiable();

        var turnCommand = new TurnCommand(turnable.Object);
        Assert.Throws<Exception>(() => turnCommand.Execute());
    }

    [Fact]
    public void CantDetermineDelta()
    {
        var turnable = new Mock<ITurnable>();
        turnable.SetupGet(m => m.angle).Returns(new Rational(45)).Verifiable();
        turnable.SetupGet(m => m.delta).Throws(new Exception()).Verifiable();

        var turnCommand = new TurnCommand(turnable.Object);
        Assert.Throws<Exception>(() => turnCommand.Execute());
    }

    [Fact]
    public void CantChangeAngle()
    {
        var turnable = new Mock<ITurnable>();
        turnable.SetupGet(m => m.angle).Returns(new Rational(45)).Verifiable();
        turnable.SetupGet(m => m.delta).Returns(new Rational(45)).Verifiable();
        turnable.SetupSet(m => m.angle = It.IsAny<Rational>()).Throws(new Exception()).Verifiable();
        var turnCommand = new TurnCommand(turnable.Object);

        Assert.Throws<Exception>(() => turnCommand.Execute());
    }
}
