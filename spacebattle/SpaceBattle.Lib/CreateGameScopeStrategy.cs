namespace SpaceBattle.Lib
{
    public class CreateGameScopeStrategy : IStrategy
    {
        public object Invoke(object[] args)
        {
            string gameId = (string)args[0];
            var scope = (object)args[1];
            var quantum = (int)args[2];

            var gameScope = IoC.Resolve<object>("Scopes.New", scope);

            var gameScopes = IoC.Resolve<IDictionary<string, object>>("Game.Scope.Map");
            gameScopes.Add(gameId, gameScope);

            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", gameScope).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Get.Time.Quantum", (object[] args) => quantum).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Queue.Inqueue", (object[] args) => new PushByIdStrategy().Invoke(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Queue.Dequeue", (object[] args) => new DequeueByGameIdStrategy().Invoke(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Get.UObject", (object[] args) => new GetUObjectByGameIdStrategy().Invoke(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Delete.UObject", (object[] args) => new DeleteUObjectByGameIdStrategy().Invoke(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

            return gameScope;
        }
    }
}
