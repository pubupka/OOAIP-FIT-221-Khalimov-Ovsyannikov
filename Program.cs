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
            var listOfThreads = IoC.Resolve<List<ICommand>>("Server.Start.AllThreads", int.Parse(Console.ReadLine()));
            Console.ReadKey();
            IoC.Resolve<ICommand>("Server.Stop.AllThreads", dictOfThreads);
        }
    }
}