using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceBattle.Lib.Tests
{
    public class CreateAndFindAdapterTests
    {
        public CreateAndFindAdapterTests()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();

            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        }

        [Fact]
        public void CreateAndFindAdapterTestsPositive()
        {
            var mockIUobj = new Mock<IUObject>();
            var targetType = new Mock<IMovable>().Object.GetType();

            var dictOfAssemblies = new Dictionary<KeyValuePair<Type, Type>, Assembly>();
            var references = new List<MetadataReference> {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(Assembly.Load("SpaceBattle.Lib").Location)
            };

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.CreateAdapterStrategy", (object[] args) => new CreateAdapterStrategy().Invoke(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Get.DictOfAssemblies", (object[] args) => dictOfAssemblies).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GenerateAdapter", (object[] args) => new CompileCodeCommandStrategy().Invoke(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.CompileCodeCommand", (object[] args) =>
            {
                var objType = (Type)args[0];
                var targetType = (Type)args[1];
                return new CompileCodeOfAdapterCommand(objType, targetType);
            }).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Code", (object[] args) =>
            {
                return
            @"namespace SpaceBattle.Lib;
            public class IMovableAdapter : IMovable
            {
                private IUObject _uObject;
                public IMovableAdapter(IUObject uObject) { _uObject = uObject;}
                public Vector Position
                {
                    get => new Vector(new int[] { 0, 0 });
                    set => new Vector(new int[] { 0, 0 });
                }
                public Vector Velocity
                {
                    get => new Vector(new int[] { 1, 1 });
                }
            }
            ";
            }).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Code.Compile", (object[] args) => new CompileCodeStrategy().Invoke(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Assembly.Name.Create", (object[] args) => Guid.NewGuid().ToString()).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Compile.References", (object[] args) => references).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.TypesAssemblyDict.AddNewElement", (object[] args) =>
            {
                var key = new KeyValuePair<Type, Type>((Type)args[0], (Type)args[1]);

                return new ActionCommand(() => { dictOfAssemblies.Add(key, (Assembly)args[2]); });
            }).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.FindAdapter", (object[] args) => new FindAdapterStrategy().Invoke(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Adapter.Name", (object[] args) =>
            {
                return "SpaceBattle.Lib.IMovable" + "Adapter";
            }).Execute();

            var obj = IoC.Resolve<IMovable>("Game.CreateAdapterStrategy", mockIUobj.Object, targetType);

            Assert.NotNull(obj);

            dictOfAssemblies.TryGetValue(new KeyValuePair<Type, Type>(mockIUobj.Object.GetType(), targetType), out var assembly);
            Assert.Equal("SpaceBattle.Lib.IMovableAdapter", assembly!.GetType(IoC.Resolve<string>("Game.Adapter.Name", targetType))!.ToString());

            Assert.Equal(obj.Position, new Vector(new int[] { 0, 0 }));
        }
    }
}