using System;
using GameLibrary;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;

namespace ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
    #if DEBUG
            .AddJsonFile("settings.Debug.json")
    #else
                .AddJsonFile("settings.json")
    #endif
                .Build();
            
            var server = new Server(new TelegramBotClient(config["Key"]), null).Start();
            Console.ReadKey();
        }
    }
}