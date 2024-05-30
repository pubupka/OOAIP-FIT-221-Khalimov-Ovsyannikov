using System.Collections;
using Hwdtech;

namespace SpaceBattle.Lib
{
    public class InitEmptyUObjectsCommand : ICommand
    {
        private readonly int _count;
        public InitEmptyUObjectsCommand(int count)
        {
            _count = count;
        }

        public void Execute()
        {
            var uObjectsTable = IoC.Resolve<Hashtable>("Game.GetUObjects");

            Enumerable.Range(0, _count).ToList().ForEach(index => {
                uObjectsTable.Add(index, IoC.Resolve<IUObject>("Game.UObject.Empty.Create"));
            });
        }
    }
}
