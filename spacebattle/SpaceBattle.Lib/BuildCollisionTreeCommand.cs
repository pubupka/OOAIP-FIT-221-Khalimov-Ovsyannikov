using Hwdtech;

public class BuildCollisionTreeCommand : ICommand
{
    private readonly IArraysFromFileReader _reader;

    public BuildCollisionTreeCommand(IArraysFromFileReader reader)
    {
        _reader = reader;
    }

    public void Execute()
    {
        var arrays = _reader.ReadArrays();

        arrays.ForEach(array =>
        {
            var node = IoC.Resolve<Dictionary<int, object>>("Game.Collisions.Tree");
            array.ToList().ForEach(num =>
            {
                node.TryAdd(num, new Dictionary<int, object>());
                node = (Dictionary<int, object>)node[num];
            });
        });
    }
}
