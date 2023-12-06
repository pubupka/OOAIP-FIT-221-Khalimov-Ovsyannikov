using System.Collections;
using Hwdtech;

public class CollisionTreeBuilder
{
    private readonly List<int[]> _arrays;

    public CollisionTreeBuilder(string path)
    {
        _arrays = IoC.Resolve<CollisionTextFile_WithSpaceAsSeparator_Reader>("Game.Collisions.TreeBuilder", path).ReadArrays();

        _arrays.ForEach(array => {
            var node = IoC.Resolve<Hashtable>("Game.Collisions.Tree");
            array.ToList().ForEach(num => {
                if (!node.ContainsKey(num))
                {
                    node.Add(num, new Hashtable());
                }
                node = (Hashtable)node[num];
            });
        });
    }
}
