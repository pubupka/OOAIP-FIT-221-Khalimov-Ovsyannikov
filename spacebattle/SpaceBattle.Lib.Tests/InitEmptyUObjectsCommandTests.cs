using System.Collections;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib
{
    public class InitEmptyUObjectsCommandTests
    {
        public InitEmptyUObjectsCommandTests()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        }

        [Fact]
        public void InitEmptyUObjectsCommand_Positive()
        {
            var count = 3;
            var ht = new Hashtable();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetUObjects", (object[] args) => ht).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.UObject.Empty.Create", (object[] args) => new Mock<IUObject>().Object).Execute();

            new InitEmptyUObjectsCommand(count).Execute();
            Assert.Equal(count, ht.Count);
        }

        [Fact]
        public void InitEmptyUObjectsCommand_GetUObjects_Failed()
        {
            var count = 3;
            var strategy = new Mock<IStrategy>();
            strategy.Setup(s => s.Invoke()).Throws<NotImplementedException>();
            var ht = new Hashtable();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetUObjects", (object[] args) => strategy.Object.Invoke()).Execute();

            var initCmd = new InitEmptyUObjectsCommand(count);
            Assert.Throws<NotImplementedException>(initCmd.Execute);
        }

        [Fact]
        public void InitEmptyUObjectsCommand_CreateEmptyUObject_Failed()
        {
            var count = 3;
            var ht = new Hashtable();
            var strategy = new Mock<IStrategy>();
            strategy.Setup(s => s.Invoke()).Throws<NotImplementedException>();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetUObjects", (object[] args) => ht).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.UObject.Empty.Create", (object[] args) => strategy.Object.Invoke()).Execute();

            var initCmd = new InitEmptyUObjectsCommand(count);
            Assert.Throws<NotImplementedException>(initCmd.Execute);
        }
    }
}
