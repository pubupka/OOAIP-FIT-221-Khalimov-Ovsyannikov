using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests
{
    public class EndCommandTests
    {
        [Fact]
        public void IQueueTestExample()
        {
            var qReal = new Queue<ICommand>();

            var qMock = new Mock<IQueue>();
            qMock.Setup(q => q.Take()).Returns(()=>qReal.Dequeue());

            var cmd = new Mock<ICommand>();

            qReal.Enqueue(cmd.Object);

            Assert.Equal(cmd.Object, qMock.Object.Take());
        }
        
        [Fact]
        void IoCIQueue() 
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            var qReal = new Queue<ICommand>();
            var qMock = new Mock<IQueue>();
            qMock.Setup(q => q.Take()).Returns(()=>qReal.Dequeue());
            qMock.Setup(q => q.Add(It.IsAny<ICommand>())).Callback(
                (ICommand cmd) => {
                qReal.Enqueue(cmd);
                }
            );

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register", 
                "Game.Queue", 
                (object[] args)=> {
                    return qMock.Object;
                }
            ).Execute();

            var cmd = new Mock<ICommand>();

            IoC.Resolve<IQueue>("Game.Queue").Add(cmd.Object);
        
            var cmdFromIoC = IoC.Resolve<IQueue>("Game.Queue").Take();

            Assert.Equal(cmd.Object, cmdFromIoC);
        }
    }
}
