using System.Collections;
using Hwdtech;

public class BuildCollisionTreeCommand: ICommand
{
    private readonly List<int[]> _arrays;

    public BuildCollisionTreeCommand(string path)
    {
        _arrays = IoC.Resolve<CollisionTextFile_WithSpaceAsSeparator_Reader>("Game.Collisions.TreeBuilder", path).ReadArrays();
    }

    public void Execute()
    {
        _arrays.ForEach(array => {
            var node = IoC.Resolve<Hashtable>("Game.Collisions.Tree");
            array.ToList().ForEach(num => {
                _ = node.ContainsKey(num) ? node[num] : node[num] = new Hashtable();
                // if (!node.ContainsKey(num))
                // {
                //     node.Add(num, new Hashtable());
                // }

                node = (Hashtable)node[num]!;
            });
        });
    }
}
