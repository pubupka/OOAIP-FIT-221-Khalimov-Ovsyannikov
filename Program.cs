using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hwdtech;

namespace SpaceBattle.Lib
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите кол-во потоков");
            IoC.Resolve<ICommand>("Server.Start.AllThreads", Console.ReadLine()).Execute();
            Console.ReadKey();
            IoC.Resolve<ICommand>("Server.Stop.AllThreads").Execute();
        }
    }
}