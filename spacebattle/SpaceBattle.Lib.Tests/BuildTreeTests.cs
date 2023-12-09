using D = System.Collections.Generic.Dictionary<int, object>;
using Hwdtech;
using Hwdtech.Ioc;

public class BuildTreeTests
{
    public BuildTreeTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        var tree = new D();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Collisions.Tree", (object[] args) => {
            return tree;
        }).Execute();
    }

    [Fact]
    public void BuildTree_Positive()
    {
        var reader = new Mock<IArraysFromFileReader>();
        var path = "../../../CollisionVectors.txt";
        var arrays = File.ReadAllLines(path).Select(
            line => line.Split(" ").Select(num => int.Parse(num)).ToArray()
        ).ToList();

        var expected_layer_1 = new HashSet<int>() { 1, 6 };
        var expected_layer_2 = new HashSet<int>() { 2, 7 };
        var expected_layer_3 = new HashSet<int>() { 3, 8 };
        var expected_layer_4 = new HashSet<int>() { 4, 5, 9 };

        // var layer_numbers = new List<int> { 0, 1, 2, 3 };
        // var expected_keys_by_layers = layer_numbers.Select(
        //     layer_number => arrays.Select(
        //         array => array[layer_number]
        //     ).ToHashSet()
        // ).ToList();  // Список множеств. Первое множество - ключи, которые должны быть на первом слое, второе - ключи на втором слое и т.д.

        reader.Setup(r => r.ReadArrays()).Returns(arrays);

        var cmd = new BuildCollisionTreeCommand(reader.Object);
        cmd.Execute();

        var tree = IoC.Resolve<D>("Game.Collisions.Tree");

        var real_layer_1 = tree.Keys;
        var real_layer_2 = ((D)tree[1]).Keys.Union(((D)tree[6]).Keys);
        var real_layer_3 = ((D)((D)tree[1])[2]).Keys.Union(((D)((D)tree[6])[7]).Keys);
        var real_layer_4 = ((D)((D)((D)tree[1])[2])[3]).Keys.Union(((D)((D)((D)tree[6])[7])[8]).Keys);

        Assert.True(expected_layer_1.SequenceEqual(real_layer_1));
        Assert.True(expected_layer_2.SequenceEqual(real_layer_2));
        Assert.True(expected_layer_3.SequenceEqual(real_layer_3));
        Assert.True(expected_layer_4.SequenceEqual(real_layer_4));

        // Добавить 4 слоя ключей, убедиться что на каждом слое присутствуют нужные ключи
    }
}
