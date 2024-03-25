using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hwdtech;

namespace SpaceBattle.Lib
{
    public class RunMainCommand: ICommand
    {
        private int _count;
        public RunMainCommand(int count)
        {
            _count = count;
        }
        public void Execute()
        {
            Console.WriteLine($"Запуск {_count}-ти поточного сервера");
            var barrier = IoC.Resolve<Barrier>("Server.MakeBarrier", _count);

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register","Server.TakeBarrier",(object[] args)=>{
                return barrier;
            }).Execute();

            IoC.Resolve<ICommand>("Server.Start", _count).Execute();
            Console.WriteLine($"{_count}-ти поточный сервер запущен");

            Console.Read();

            Console.WriteLine($"Остановка {_count}-ти поточного сервера");
            IoC.Resolve<ICommand>("Server.Stop").Execute();
            barrier.SignalAndWait();

            Console.WriteLine($"{_count}-ти поточный сервер остановлен");
        }
    }
}
