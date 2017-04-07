using System;

namespace BuilderService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Started Builder");
            new Worker().ProcessQueue();
            Console.WriteLine("End Builder");
            Console.ReadLine();
        }
    }
}