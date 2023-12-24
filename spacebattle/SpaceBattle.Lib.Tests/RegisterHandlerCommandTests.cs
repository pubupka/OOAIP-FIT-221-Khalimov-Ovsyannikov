using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests
{
    public class RegisterHandlerCommandTests
    {
        public RegisterHandlerCommandTests()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        }

        [Fact]
        public void SuccesfullRegisterHandlerCommand()
        {
            var mockHandler = new Mock<IHandler>();
            var mockCommand = new Mock<ICommand>();

            var mockExceptionHandlerTree = new Mock<IDictionary<string, IHandler>>();
            var realDict = new Dictionary<string, IHandler>();
            mockExceptionHandlerTree.Setup(x => x.Add(It.IsAny<string>(), It.IsAny<IHandler>())).Callback((string key, IHandler handler)=>realDict.Add(key, handler)).Verifiable();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.ExceptionHandler.Register", (object[] args) => new RegisterHandlerCommand((IEnumerable<Type>)args[0], (IHandler)args[1])).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.ExceptionHandler.Tree", (object[] args) => mockExceptionHandlerTree.Object).Execute();

            IoC.Resolve<ICommand>("Game.ExceptionHandler.Register", new Type[] { typeof(ICommand), typeof(Exception) }, mockHandler.Object).Execute();

            mockExceptionHandlerTree.VerifyAll();
            Assert.Single(realDict);
            Assert.True(realDict.ContainsValue(mockHandler.Object));
        }

        [Fact]
        public void RegisterExistedHandlerCommand()
        {
            var mockHandler = new Mock<IHandler>();
            var mockCommand = new Mock<ICommand>();

            var mockExceptionHandlerTree = new Mock<IDictionary<string, IHandler>>();
            var realDict = new Dictionary<string, IHandler>();
            var exit = mockHandler.Object;
            
            mockExceptionHandlerTree.Setup(x => x.Add(It.IsAny<string>(), It.IsAny<IHandler>())).Callback((string key, IHandler handler)=>realDict.Add(key, handler)).Verifiable();
            mockExceptionHandlerTree.Setup(x => x.TryGetValue(It.IsAny<string>(), out exit)).Returns(true);

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.ExceptionHandler.Register", (object[] args) => new RegisterHandlerCommand((IEnumerable<Type>)args[0], (IHandler)args[1])).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.ExceptionHandler.Tree", (object[] args) => mockExceptionHandlerTree.Object).Execute();

            IoC.Resolve<ICommand>("Game.ExceptionHandler.Register", new Type[] { typeof(ICommand), typeof(Exception) }, mockHandler.Object).Execute();

            mockExceptionHandlerTree.Verify(x => x.TryGetValue(It.IsAny<string>(), out exit), Times.Once);
            Assert.False(realDict.ContainsValue(mockHandler.Object));
        }

        [Fact]
        public void SuccesfullRegisterSomeHandlersCommand()
        {
            var mockHandler = new Mock<IHandler>();
            var mockCommand = new Mock<ICommand>();

            var mockExceptionHandlerTree = new Mock<IDictionary<string, IHandler>>();
            var realDict = new Dictionary<string, IHandler>();
            mockExceptionHandlerTree.Setup(x => x.Add(It.IsAny<string>(), It.IsAny<IHandler>())).Callback((string key, IHandler handler)=>realDict.Add(key, handler)).Verifiable();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.ExceptionHandler.Register", (object[] args) => new RegisterHandlerCommand((IEnumerable<Type>)args[0], (IHandler)args[1])).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.ExceptionHandler.Tree", (object[] args) => mockExceptionHandlerTree.Object).Execute();

            IoC.Resolve<ICommand>("Game.ExceptionHandler.Register", new Type[] { typeof(ICommand), typeof(Exception) }, mockHandler.Object).Execute();
            IoC.Resolve<ICommand>("Game.ExceptionHandler.Register", new Type[] { typeof(IQueue), typeof(Exception) }, mockHandler.Object).Execute();
            IoC.Resolve<ICommand>("Game.ExceptionHandler.Register", new Type[] { typeof(IQueue), typeof(System.Collections.Generic.KeyNotFoundException) }, mockHandler.Object).Execute();

            mockExceptionHandlerTree.VerifyAll();
            Assert.Equal(3, realDict.Count());
        }
    }
}