using Hwdtech;

namespace SpaceBattle.Lib
{
    public class SettingPropertiesAndReturningCommandByMessageStrategy : IStrategy
    {
        public object Invoke(object[] args)
        {
            var message = (IProcessable)args[0];
            var gameId = message.gameId;
            var itemId = message.gameItemId;
            var attributes = message.attributes;
            var gameItemsdict = IoC.Resolve<Dictionary<string, Dictionary<int, IUObject>>>("Server.GameItemsdict");
            var item = gameItemsdict[gameId][itemId];
            attributes.ToList().ForEach(atr => item.SetProperty(atr.Key, atr.Value));
            return IoC.Resolve<ICommand>("Game.Command." + message.cmdType, item);
        }
    }
}
