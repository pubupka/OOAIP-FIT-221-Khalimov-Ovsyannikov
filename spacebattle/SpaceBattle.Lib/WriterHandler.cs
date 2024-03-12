namespace SpaceBattle.Lib
{
    public class WriterHandler : IHandler
    {
        private readonly ICommand _cmd;
        private readonly Exception _exception;
        public WriterHandler(ICommand cmd, Exception exception)
        {
            _cmd = cmd;
            _exception = exception;
        }
        public void Handle()
        {
            var path = "../../../Log.txt";
            var sw = new StreamWriter(path);
            sw.WriteLine($"При выполнении команды < {_cmd.GetType()} > возникло исключение < {_exception.GetType()} >");
            sw.Close();
        }
    }
}
