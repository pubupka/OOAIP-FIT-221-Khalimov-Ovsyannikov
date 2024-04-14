using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests
{
    public class WriteHandlerTests
    {

        public WriteHandlerTests()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();

            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New",
            IoC.Resolve<object>("Scopes.Root"))).Execute();
        }
        [Fact]
        public void WriteHandlerPositiveTest()
        {
            var cmd = new EmptyCommand();
            var exc = new DivideByZeroException();
            var path = Path.GetTempFileName();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GetReportInfo", (object[] args) =>
            {
                return (string)new GetReportStrategy().Invoke(args);
            }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GetLogFilePath", (object[] args) =>
            {
                return path;
            }).Execute();

            var handler = new WriterHandler(cmd, exc);
            handler.Handle();

            var str = File.ReadLines(path).Last();
            Assert.Contains("При выполнении команды < SpaceBattle.Lib.EmptyCommand > возникло исключение < System.DivideByZeroException >", str);
        }
    }
}
