using Hwdtech;

public class CollisionTextFile_WithSpaceAsSeparator_Reader: IVectorsFromFileReader
{
    private readonly string _path;
    
    public CollisionTextFile_WithSpaceAsSeparator_Reader(string path)
    {
        _path = path;
    }

    public List<Vector> ReadVectors()
    {
        var lines = IoC.Resolve<List<string>>("Game.Collisions.LoadData", _path);

        var result_array = IoC.Resolve<List<Vector>>("Game.Collisions.ConvertDataToListOfVectors", lines);
        // Vector[] result_array;
        // result_array = file.ToList().ForEach(line => line.Split(" "))
        return result_array;
    }
}
