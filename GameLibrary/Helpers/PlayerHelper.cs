using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace GameLibrary.Helpers
{
    public static class PlayerHelper
    {
        public static List<KeyboardButton> GetKeyboard(this Player player)
        {
            if (player.Lvl < 5)
            {
                return new List<KeyboardButton>()
                {
                    new KeyboardButton("👤Профиль"), new KeyboardButton("📃Приключения"), new KeyboardButton("🏕Поляна"), new KeyboardButton("🛠Крафт"), new KeyboardButton("💬О игре")
                };
            }
            else
            {
                return new List<KeyboardButton>()
                {
                    new KeyboardButton("👤Профиль"), new KeyboardButton("📃Приключения"),
                    new KeyboardButton("🏕Поляна"), new KeyboardButton("🏰Город"), new KeyboardButton("🛠Крафт"), new KeyboardButton("💬О игре")
                };
            }
        }

        public static void RefreshKeyboard(this Player player, TelegramBotClient client)
        {
            client.SendTextMessageAsync(player.Id, "Обновили :)",
                replyMarkup: new ReplyKeyboardMarkup(player.GetKeyboard().SplitList()));
        }
    }
}