using System;
using GameLibrary;
using Telegram.Bot;

namespace ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new Server(new TelegramBotClient("434659716:AAEoZ-9s0c5n6s_7GWVGQJbzDsCS5RobPGM"), null).Start();
            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}