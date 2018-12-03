using System.Collections.Concurrent;
using Telegram.Bot;

namespace GameLibrary.Helpers
{
    public class Helper
    {
        public static TelegramBotClient Client { get; set; }
        public static ConcurrentQueue<CommandResult> Queue { get; } = new ConcurrentQueue<CommandResult>();

        public static void Print(string message, Player player)
        {
            
        }
    }
}