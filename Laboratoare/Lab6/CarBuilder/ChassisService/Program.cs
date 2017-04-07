using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace ChassisService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Started Chassis");
            new Worker().ProcessQueue();
            Console.WriteLine("End Chassis");
            Console.ReadLine();

        }
    }
}