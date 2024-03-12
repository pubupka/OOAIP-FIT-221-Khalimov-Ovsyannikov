using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceBattle.Lib.Tests
{
    public class WriteHandlerTests
    {
        [Fact]
        public void WriteHandlerPositiveTest()
        {
            var cmd = new InitStartAndStopServerCommand();
            Exception exc = new DivideByZeroException();

            WriterHandler handler = new WriterHandler(cmd, exc);
            handler.Handle();
            
            var path = "../../../Log.txt";
            var str = File.ReadLines(path);
            Assert.True(str.Contains("При выполнении команды < SpaceBattle.Lib.InitStartAndStopServerCommand > возникло исключение < System.DivideByZeroException >"));
        }
    }
}