using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests
{
    public class TestRealRegisterHandler
    {
        public TestRealRegisterHandler()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        }

        [Fact]
        public void RegisterHandlerCommandPositive()
        {
            var mockHandler = new Mock<IRealHandler>();
            var mockCommand = new Mock<ICommand>();

            var listOfTypes = new List<Type> { typeof(ICommand), typeof(Exception) };

            var mockExceptionHandlerTree = new Mock<IDictionary<object, IRealHandler>>();
            var realDict = new Dictionary<object, IRealHandler>();
            
            mockExceptionHandlerTree.Setup(x => x.Add(It.IsAny<object>(), It.IsAny<IRealHandler>())).Callback((object key, IRealHandler handler) => realDict.TryAdd(key, handler));

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.ExceptionHandler.Register", (object[] args) => new RealRegisterHandlerCommand((IEnumerable<Type>)args[0], (IRealHandler)args[1])).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.ExceptionHandler.Tree", (object[] args) => mockExceptionHandlerTree.Object).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.HashCode", (object[] args) => 
            {
                object output = new GetHashStrategy(listOfTypes).Invoke();
                return output;}).Execute();

            IoC.Resolve<SpaceBattle.Lib.RealRegisterHandlerCommand>("Game.ExceptionHandler.Register", listOfTypes, mockHandler.Object).Execute();

            Assert.Single(realDict);
            Assert.True(realDict.ContainsValue(mockHandler.Object));
        }

    }
}