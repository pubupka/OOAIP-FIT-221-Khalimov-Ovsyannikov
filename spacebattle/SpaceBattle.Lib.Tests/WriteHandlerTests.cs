namespace SpaceBattle.Lib.Tests
{
    public class WriteHandlerTests
    {
        [Fact]
        public void WriteHandlerPositiveTest()
        {
            var cmd = new InitStartAndStopServerCommand();
            Exception exc = new DivideByZeroException();

            var handler = new WriterHandler(cmd, exc);
            handler.Handle();

            var path = "../../../Log.txt";
            var str = File.ReadLines(path);
            Assert.Contains("При выполнении команды < SpaceBattle.Lib.InitStartAndStopServerCommand > возникло исключение < System.DivideByZeroException >", str);
        }
    }
}
