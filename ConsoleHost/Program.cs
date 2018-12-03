using System;
using System.IO;
using GameLibrary;
using GameLibrary.Controllers;
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
            var client = new TelegramBotClient(config["Key"]);
            
            var server = new Server(client, config["DBKey"])
                .Add(new Start())
                .Add(new Help(client))
                .Add(new PlayerTasks())
                .Add(new Profile())
                .Add(new Bag())
                .Add(new Items())
                .Start();
            Console.ReadKey();
        }
    }
}