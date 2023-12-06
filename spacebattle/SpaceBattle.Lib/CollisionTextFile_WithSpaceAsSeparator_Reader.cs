using Hwdtech;

public class CollisionTextFile_WithSpaceAsSeparator_Reader: IArraysFromFileReader
{
    private readonly string _path;
    
    public CollisionTextFile_WithSpaceAsSeparator_Reader(string path)
    {
        _path = path;
    }

    public List<int[]> ReadArrays()
    {
        return IoC.Resolve<List<int[]>>("Game.Collisions.LoadCollisionsTextFile", _path);
    }
}
