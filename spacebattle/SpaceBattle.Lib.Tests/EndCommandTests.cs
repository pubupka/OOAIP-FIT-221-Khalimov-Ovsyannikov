using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests
{
    public class EndCommandTests
    {
        private static void EndCommandStartTest()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Inject", (object[] args) =>
            {
                var target = (IInjectable)args[0];
                var injectedCommand = (ICommand)args[1];
                target.Inject(injectedCommand);
                return target;
            }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.EmptyCommand", (object[] args) => new EmptyCommand()).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.EndMovement", (object[] args) => { return new EndMovementCommand((IEndable)args[0]); }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.UObject.DeleteProperty", (object[] args) =>
            {
                var target = (IUObject)args[0];
                var properties = (List<string>)args[1];
                properties.ForEach(prop => target.DeleteProperty(prop));
                return "";
            }).Execute();
        }
    }
}
