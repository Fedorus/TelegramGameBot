using System;
using Telegram.Bot;
namespace GameLibrary
{
    public class Server
    {
        private TelegramBotClient _client;
        public Server(TelegramBotClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public Server AddCommand(params Command[] commands)
        {
        }
    }
}