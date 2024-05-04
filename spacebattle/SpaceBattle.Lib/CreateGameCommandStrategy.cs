using Hwdtech;

namespace SpaceBattle.Lib
{
    public class CreateGameCommandStrategy : IStrategy
    {
        public object Invoke(object[] args)
        {
            var gameId = (string)args[0];
            var parentScope = (object)args[1];
            var quantum = (int)args[2];

            var gameScope = IoC.Resolve<object>("Game.Scope.New", gameId, parentScope, quantum);
            var gameQueue = IoC.Resolve<Queue<ICommand>>("Game.Queue.New");
            var gameCmd = IoC.Resolve<ICommand>("Game.Command", gameQueue, gameScope);

            var cmdList = new List<ICommand> { gameCmd };
            var macroCmd = IoC.Resolve<ICommand>("Game.Command.Macro", cmdList);
            var injectCmd = IoC.Resolve<ICommand>("Game.Command.Inject", macroCmd);
            var repeatConcurrentCmd = IoC.Resolve<ICommand>("Command.Concurrent.Repeat", injectCmd);
            cmdList.Add(repeatConcurrentCmd);

            var gameDict = IoC.Resolve<IDictionary<string, ICommand>>("Game.Dict");
            gameDict.Add(gameId, injectCmd);

            return injectCmd;
        }
    }
}
