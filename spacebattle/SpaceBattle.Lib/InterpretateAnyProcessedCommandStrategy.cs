using Hwdtech;

namespace SpaceBattle.Lib
{
    public class InterpretateAnyProcessedCommandStrategy : IStrategy
    {
        public object Invoke(object[] args)
        {
            var gameId = (string)args[0];
            var itemId = (int)args[1];
            var item = IoC.Resolve<IUObject>("Server.ItemById", gameId, itemId);
            var processedCmds = (List<string>)args[2];
            var listOfCmds = processedCmds.Select(x => IoC.Resolve<ICommand>("Game.Command." + x, item)).ToList<ICommand>();

            return IoC.Resolve<ICommand>("Server.Command.PushByGameIdCommand", gameId, IoC.Resolve<ICommand>("Game.Command.MacroCommand", listOfCmds));
        }
    }
}
