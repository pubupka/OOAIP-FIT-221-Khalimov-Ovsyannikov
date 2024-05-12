using System.Collections.Concurrent;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests
{
    public class InterpretateAnyProcessedCommandStrategyTests
    {
        public InterpretateAnyProcessedCommandStrategyTests()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        }

        [Fact]
        public void InterpretateAnyProcessedCommandStrategyPositive()
        {
            var gameId = "absd";
            var itemId = 1;
            var item = new Mock<IUObject>();
            var listOfCmds = new List<string>() { "StartMovementCommand", "TurnCommand", "EndMovement" };
            var gameQueue = new BlockingCollection<ICommand>();
            var mockStartCmd = new Mock<ICommand>();
            var mockTurnCmd = new Mock<ICommand>();
            var mockEndCmd = new Mock<ICommand>();
            mockStartCmd.Setup(x => x.Execute()).Verifiable();
            mockTurnCmd.Setup(x => x.Execute()).Verifiable();
            mockEndCmd.Setup(x => x.Execute()).Verifiable();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.StartMovementCommand", (object[] args) => mockStartCmd.Object).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.TurnCommand", (object[] args) => mockTurnCmd.Object).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.EndMovement", (object[] args) => mockEndCmd.Object).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.ItemById", (object[] args) => item.Object).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.MacroCommand", (object[] args) => new MacroComand((List<ICommand>)args[0])).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Command.PushByGameIdCommand", (object[] args) => new ActionCommand(() => { gameQueue.Add((ICommand)args[1]); })).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.InterpretateAnyProcessedCommand", (object[] args) => new InterpretateAnyProcessedCommandStrategy().Invoke(args)).Execute();
            IoC.Resolve<ICommand>("Server.InterpretateAnyProcessedCommand", gameId, itemId, listOfCmds).Execute();
            gameQueue.Take().Execute();

            mockStartCmd.Verify(x => x.Execute(), Times.Once());
            mockTurnCmd.Verify(x => x.Execute(), Times.Once());
            mockEndCmd.Verify(x => x.Execute(), Times.Once());
        }

        [Fact]
        public void CommandCantBeAppliedToObject()
        {
            var gameId = "absd";
            var itemId = 1;
            var item = new Mock<IUObject>();
            var listOfCmds = new List<string>() { "StartMovementCommand" };
            var gameQueue = new BlockingCollection<ICommand>();
            var mockStartCmd = new Mock<ICommand>();
            var mockTurnCmd = new Mock<ICommand>();
            var mockEndCmd = new Mock<ICommand>();
            mockStartCmd.Setup(x => x.Execute()).Verifiable();
            mockTurnCmd.Setup(x => x.Execute()).Verifiable();
            mockEndCmd.Setup(x => x.Execute()).Verifiable();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.StartMovementCommand", (object[] args) => new StartMoveCommand((IMoveStartable)args[0])).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.ItemById", (object[] args) => item.Object).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.InterpretateAnyProcessedCommand", (object[] args) => new InterpretateAnyProcessedCommandStrategy().Invoke(args)).Execute();
            Assert.Throws<InvalidCastException>(() => IoC.Resolve<ICommand>("Server.InterpretateAnyProcessedCommand", gameId, itemId, listOfCmds));
        }

        [Fact]
        public void NonExistentItemId()
        {
            var gameId = "absd";
            var itemId = 1;
            var item = new Mock<IUObject>();
            var listOfCmds = new List<string>() { "StartMovementCommand" };
            var gameQueue = new BlockingCollection<ICommand>();
            var gameCmdDict = new Dictionary<string, Dictionary<int, IUObject>>() { { "absd", new Dictionary<int, IUObject>() { { 0, item.Object } } } };
            var mockStartCmd = new Mock<ICommand>();
            var mockTurnCmd = new Mock<ICommand>();
            var mockEndCmd = new Mock<ICommand>();
            mockStartCmd.Setup(x => x.Execute()).Verifiable();
            mockTurnCmd.Setup(x => x.Execute()).Verifiable();
            mockEndCmd.Setup(x => x.Execute()).Verifiable();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.ItemById", (object[] args) => gameCmdDict[(string)args[0]][(int)args[1]]).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.InterpretateAnyProcessedCommand", (object[] args) => new InterpretateAnyProcessedCommandStrategy().Invoke(args)).Execute();
            Assert.Throws<KeyNotFoundException>(() => IoC.Resolve<ICommand>("Server.InterpretateAnyProcessedCommand", gameId, itemId, listOfCmds));
        }

        [Fact]
        public void NonExistentGameId()
        {
            var gameId = "abs";
            var itemId = 1;
            var item = new Mock<IUObject>();
            var listOfCmds = new List<string>() { "StartMovementCommand" };
            var gameQueue = new BlockingCollection<ICommand>();
            var gameCmdDict = new Dictionary<string, Dictionary<int, IUObject>>() { { "absd", new Dictionary<int, IUObject>() { { 0, item.Object } } } };
            var mockStartCmd = new Mock<ICommand>();
            var mockTurnCmd = new Mock<ICommand>();
            var mockEndCmd = new Mock<ICommand>();
            mockStartCmd.Setup(x => x.Execute()).Verifiable();
            mockTurnCmd.Setup(x => x.Execute()).Verifiable();
            mockEndCmd.Setup(x => x.Execute()).Verifiable();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.ItemById", (object[] args) => gameCmdDict[(string)args[0]][(int)args[1]]).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.InterpretateAnyProcessedCommand", (object[] args) => new InterpretateAnyProcessedCommandStrategy().Invoke(args)).Execute();
            Assert.Throws<KeyNotFoundException>(() => IoC.Resolve<ICommand>("Server.InterpretateAnyProcessedCommand", gameId, itemId, listOfCmds));
        }

        [Fact]
        public void CantPushCommandByGameId()
        {
            var gameId = "absd";
            var itemId = 1;
            var item = new Mock<IUObject>();
            var listOfCmds = new List<string>() { "StartMovementCommand", "TurnCommand", "EndMovement" };
            var mockStartCmd = new Mock<ICommand>();
            var mockTurnCmd = new Mock<ICommand>();
            var mockEndCmd = new Mock<ICommand>();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.StartMovementCommand", (object[] args) => mockStartCmd.Object).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.TurnCommand", (object[] args) => mockTurnCmd.Object).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.EndMovement", (object[] args) => mockEndCmd.Object).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.ItemById", (object[] args) => item.Object).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.MacroCommand", (object[] args) => new MacroComand((List<ICommand>)args[0])).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Command.PushByGameIdCommand", (object[] args) => new ActionCommand(() => { throw new Exception(); })).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.InterpretateAnyProcessedCommand", (object[] args) => new InterpretateAnyProcessedCommandStrategy().Invoke(args)).Execute();
            Assert.Throws<Exception>(() => IoC.Resolve<ICommand>("Server.InterpretateAnyProcessedCommand", gameId, itemId, listOfCmds).Execute());
        }
    }
}
