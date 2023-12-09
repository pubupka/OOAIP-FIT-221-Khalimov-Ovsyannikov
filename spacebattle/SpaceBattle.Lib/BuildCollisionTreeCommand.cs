using Hwdtech;

public class BuildCollisionTreeCommand: ICommand
{
    private readonly IArraysFromFileReader _reader;

    public BuildCollisionTreeCommand(IArraysFromFileReader reader)
    {
        _reader = reader;
    }

    public void Execute()
    {
        var arrays = _reader.ReadArrays();

        arrays.ForEach(array => {
            var node = IoC.Resolve<Dictionary<int, object>>("Game.Collisions.Tree");
            array.ToList().ForEach(num => {
                node.TryAdd(num, new Dictionary<int, object>());
                node = (Dictionary<int, object>)node[num];
                
                //_ = node.ContainsKey(num) ? node[num] : node[num] = new Hashtable();
                // if (!node.ContainsKey(num))
                // {  проверку вынести в отдельное место, как варик доопределить приватный метод, аналог tryadd, который всегда будет возвращать не null
                //     node.Add(num, new Hashtable());
                // }

                //node = (Hashtable)node[num];

                // //node = TryAdd_AndGetSubTree_HashTable(num, new Hashtable(), node);
            });
        });
    }

    // private static Hashtable TryAdd_AndGetSubTree_HashTable(object key, object value, Hashtable hashtable)
    // {
    //     if (hashtable.ContainsKey(key))
    //     {
    //         hashtable.Add(key, value);
    //     }
        
    //     return (Hashtable)hashtable[key]!;
    // }
}
