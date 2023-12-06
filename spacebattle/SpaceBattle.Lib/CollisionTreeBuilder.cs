using Hwdtech;

public class CollisionTreeBuilder
{
    private readonly List<int[]> _arrays;

    public CollisionTreeBuilder(string path)
    {
        _arrays = IoC.Resolve<CollisionTextFile_WithSpaceAsSeparator_Reader>("Game.Collisions.TreeBuilder", path).ReadArrays();

        _arrays.ForEach(array => {
            array.ToList().ForEach(num => {
                
            });
        });
    }
}
