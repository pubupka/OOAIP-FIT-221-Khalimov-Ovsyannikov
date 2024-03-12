using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace SpaceBattle.Lib
{
    public class WriterHandler:IHandler
    {
        private ICommand _cmd;
        private Exception _exception;
        public WriterHandler(ICommand cmd, Exception exception)
        {
            _cmd = cmd;
            _exception = exception;
        }
        public void Handle()
        {
            var path = "../../../Log.txt";
            StreamWriter sw = new StreamWriter(path);
            sw.WriteLine($"При выполнении команды < {_cmd.GetType()} > возникло исключение < {_exception.GetType()} >");
            sw.Close();
        }
    }
}