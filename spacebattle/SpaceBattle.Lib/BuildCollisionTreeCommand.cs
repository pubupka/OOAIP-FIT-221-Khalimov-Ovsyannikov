using System.Collections;
using Hwdtech;

public class BuildCollisionTreeCommand: ICommand
{
    private readonly List<int[]> _arrays;

    public BuildCollisionTreeCommand(List<int[]> arrays)
    {
        _arrays = arrays;
    }

    public void Execute()
    {
        _arrays.ForEach(array => {
            var node = IoC.Resolve<Hashtable>("Game.Collisions.Tree");
            array.ToList().ForEach(num => {
                TryAdd_HashTable(num, new Hashtable(), node)
                
                //_ = node.ContainsKey(num) ? node[num] : node[num] = new Hashtable();
                // if (!node.ContainsKey(num))
                // {  проверку вынести в отдельное место, как варик доопределить приватный метод, аналог tryadd, который всегда будет возвращать не null
                //     node.Add(num, new Hashtable());
                // }

                node = (Hashtable)node[num];
            });
        });
    }

    private void TryAdd_HashTable(object key, object value, Hashtable hashtable)
    {
        if (!hashtable.ContainsKey(key))
        {
            hashtable.Add(key, new Hashtable());
        }
    }
}
