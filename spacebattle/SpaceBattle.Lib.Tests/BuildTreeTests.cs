using System.Collections;
using Hwdtech;
using Hwdtech.Ioc;

public class BuildTreeTests
{
    public BuildTreeTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        var tree = new Hashtable();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Collisions.Tree", () => {
            return tree;
        }).Execute();
    }

    [Fact]
    public void BuildTree_Positive()
    {
        var reader = new Mock<IArraysFromFileReader>();
        var path = "./CollisionVectors.txt";
        var arrays = File.ReadAllLines(path).Select(
            line => line.Split(" ").Select(num => int.Parse(num)).ToArray()
        ).ToList();

        reader.Setup(r => r.ReadArrays()).Returns(arrays);

        var cmd = new BuildCollisionTreeCommand(arrays);
        cmd.Execute();

        var tree = IoC.Resolve<Hashtable>("Game.Collisions.Tree");
        // Добавить 4 слоя ключей, убедиться что на каждом слое присутствуют нужные ключи
    }
}
