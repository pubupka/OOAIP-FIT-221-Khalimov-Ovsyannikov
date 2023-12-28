using Hwdtech.Ioc;
using Hwdtech;
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
        defaulthandler.Setup(h => h.Handle()).Callback(() => {});

        var subtree = new Dictionary<Type, IHandler>() { { typeof(FileNotFoundException), handler.Object } };
        var tree = new Dictionary<Type, object>() { 
                { typeof(BuildCollisionTreeCommand), subtree } 
        };

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.DefaultHandler", (object[] args) => defaulthandler.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetHandleTree", (object[] args) => tree).Execute();

        new HandlerFinder().Find(typeof(BuildCollisionTreeCommand), typeof(FileNotFoundException)).Handle();

        Assert.True(handled);
    }

    [Fact]
    public void AnyCommand_and_SpecificException_Positive()
    {
        var handled = false;
        var handler = new Mock<IHandler>();
        var defaulthandler = new Mock<IHandler>();
        handler.Setup(h => h.Handle()).Callback(() => handled = true);
        defaulthandler.Setup(h => h.Handle()).Callback(() => {});

        var subtree = new Dictionary<Type, IHandler>() { { typeof(FileNotFoundException), handler.Object } };
        var tree = new Dictionary<Type, object>() { 
                { typeof(ICommand), subtree } 
        };

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.DefaultHandler", (object[] args) => defaulthandler.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetHandleTree", (object[] args) => tree).Execute();

        new HandlerFinder().Find(typeof(BuildCollisionTreeCommand), typeof(FileNotFoundException)).Handle();

        Assert.True(handled);
    }

    [Fact]
    public void ConcreteCommand_and_AnyException_Positive()
    {
        var handled = false;
        var handler = new Mock<IHandler>();
        var defaulthandler = new Mock<IHandler>();
        handler.Setup(h => h.Handle()).Callback(() => handled = true);
        defaulthandler.Setup(h => h.Handle()).Callback(() => {});

        var subtree = new Dictionary<Type, IHandler>() { { typeof(Exception), handler.Object } };
        var tree = new Dictionary<Type, object>() { 
                { typeof(BuildCollisionTreeCommand), subtree } 
        };

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.DefaultHandler", (object[] args) => defaulthandler.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetHandleTree", (object[] args) => tree).Execute();

        new HandlerFinder().Find(typeof(BuildCollisionTreeCommand), typeof(FileNotFoundException)).Handle();

        Assert.True(handled);
    }

    [Fact]
    public void DefaultHandled_Negative()
    {
        var defauthandled = false;
        var handler = new Mock<IHandler>();
        var defaulthandler = new Mock<IHandler>();
        handler.Setup(h => h.Handle()).Callback(() => {});
        defaulthandler.Setup(h => h.Handle()).Callback(() => defauthandled = true);

        var tree = new Dictionary<Type, object>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.DefaultHandler", (object[] args) => defaulthandler.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetHandleTree", (object[] args) => tree).Execute();

        new HandlerFinder().Find(typeof(BuildCollisionTreeCommand), typeof(FileNotFoundException)).Handle();

        Assert.True(defauthandled);
    }
}
