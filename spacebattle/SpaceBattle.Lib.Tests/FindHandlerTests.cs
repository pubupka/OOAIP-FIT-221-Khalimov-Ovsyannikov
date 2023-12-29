using Hwdtech;
using Hwdtech.Ioc;
using SpaceBattle.Lib;

public class FindHandlerCommandTests
{
    /*
        1) конкретная команда и конкретное исключение
        2) любая команда и конкретное исключение
        3) конкретная команда и любое исключение
        4) никуда не попали и вернули defaulthandler
    */
    public FindHandlerCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    [Fact]
    public void SpecificCommand_and_SpecificException_Positive()
    {
        var handled = false;
        var handler = new Mock<IHandler>();
        var defaulthandler = new Mock<IHandler>();
        handler.Setup(h => h.Handle()).Callback(() => handled = true);
        defaulthandler.Setup(h => h.Handle()).Callback(() => { });

        var subtree = new Dictionary<Type, string>() { { typeof(FileNotFoundException), "Game.EmptyCommand_FileNotFound_Handler" } };
        var tree = new Dictionary<Type, object>() {
                { typeof(EmptyCommand), subtree }
        };

        new InitTreeCommand(tree).Execute();
        new InitDefaultHandlerCommand(defaulthandler.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.EmptyCommand_FileNotFound_Handler", (object[] args) =>
        {
            return handler.Object;
        }).Execute();

        new HandlerFinder().Find(new EmptyCommand(), new FileNotFoundException()).Handle();

        Assert.True(handled);
    }

    [Fact]
    public void AnyCommand_and_SpecificException_Positive()
    {
        var handled = false;
        var handler = new Mock<IHandler>();
        var defaulthandler = new Mock<IHandler>();
        handler.Setup(h => h.Handle()).Callback(() => handled = true);
        defaulthandler.Setup(h => h.Handle()).Callback(() => { });

        var subtree = new Dictionary<Type, string>() { { typeof(FileNotFoundException), "Game.AnyCommand_FileNotFound_Handler" } };
        var tree = new Dictionary<Type, object>() {
                { typeof(ICommand), subtree }
        };

        new InitTreeCommand(tree).Execute();
        new InitDefaultHandlerCommand(defaulthandler.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.AnyCommand_FileNotFound_Handler", (object[] args) =>
        {
            return handler.Object;
        }).Execute();

        new HandlerFinder().Find(new EmptyCommand(), new FileNotFoundException()).Handle();

        Assert.True(handled);
    }

    [Fact]
    public void SpecificCommand_and_AnyException_Positive()
    {
        var handled = false;
        var handler = new Mock<IHandler>();
        var defaulthandler = new Mock<IHandler>();
        handler.Setup(h => h.Handle()).Callback(() => handled = true);
        defaulthandler.Setup(h => h.Handle()).Callback(() => { });

        var subtree = new Dictionary<Type, string>() { { typeof(Exception), "Game.EmptyCommand_AnyException_Handler" } };
        var tree = new Dictionary<Type, object>() {
                { typeof(EmptyCommand), subtree }
        };

        new InitTreeCommand(tree).Execute();
        new InitDefaultHandlerCommand(defaulthandler.Object).Execute();
        
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.EmptyCommand_AnyException_Handler", (object[] args) =>
        {
            return handler.Object;
        }).Execute();

        new HandlerFinder().Find(new EmptyCommand(), new Exception()).Handle();

        Assert.True(handled);
    }

    [Fact]
    public void DefaultHandled_Negative()
    {
        var defauthandled = false;
        var handler = new Mock<IHandler>();
        var defaulthandler = new Mock<IHandler>();
        handler.Setup(h => h.Handle()).Callback(() => { });
        defaulthandler.Setup(h => h.Handle()).Callback(() => defauthandled = true);

        new InitTreeCommand(new Dictionary<Type, object>()).Execute();
        new InitDefaultHandlerCommand(defaulthandler.Object).Execute();

        new HandlerFinder().Find(new EmptyCommand(), new FileNotFoundException()).Handle();

        Assert.True(defauthandled);
    }
}
