using Hwdtech;
using SpaceBattle.Lib;

public class InitDefaultHandlerCommand : ICommand
{
    private readonly IHandler _defaultHandler;
    public InitDefaultHandlerCommand(IHandler defaultHandler)
    {
        _defaultHandler = defaultHandler;
    }

    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.DefaultHandler", (object[] args) => _defaultHandler).Execute();
    }
}
