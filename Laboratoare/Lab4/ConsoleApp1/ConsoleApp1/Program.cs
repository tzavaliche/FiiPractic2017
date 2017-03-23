using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            ReadWriteQueue.SendMessage("test");
            ReadWriteQueue.ReadMessage();
        }
    }
}