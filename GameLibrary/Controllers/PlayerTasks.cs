using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace GameLibrary.Controllers
{
    public class PlayerTasks : Controller
    {
        public override bool CheckMessage(Message eMessage, Player player)
        {
            return eMessage.Text == "📃Приключения";
        }

        public override async Task<CommandResult> ProceedMessageAsync(Message eMessage, Player player, CommandResult commandResult)
        {
            commandResult.Text.AppendLine("Доступные занятия:");
            if (player.Lvl < 5) // Приключения на поляне
            {
                commandResult.Text.AppendLine(
                    "Блин, стрёмно чёт, погуляю пока по полянке, может найду что...");
                commandResult.Buttons.Add(new InlineKeyboardButton(){Text = "Обыскать поляну", CallbackData = "/meadow_search"});
            }
            if (player.Lvl > 2 && player.Lvl < 8) // Приключения в лесу
            {
                commandResult.Text.AppendLine(
                    "Может по краю леса пройти? Ну, я же храбный... наверно...");
                commandResult.Buttons.Add(new InlineKeyboardButton(){Text = "Походить по лесу", CallbackData = "/forest_search"});
            }

            if (player.Lvl >= 5)
            {
                commandResult.Text.AppendLine("Кажется я достаточно прокачался что бы зайти в лес подальше.");
                commandResult.Buttons.Add(new InlineKeyboardButton(){Text = "Углубиться в лес", CallbackData = "/forest_deep_search"});
            }
            return commandResult;
        }

        public override bool CheckCallback(CallbackQuery eCallbackQuery, Player player)
        {
            return (eCallbackQuery.Data == "/meadow_search" || eCallbackQuery.Data == "/forest_search" ||
                    eCallbackQuery.Data == "/forest_deep_search");
        }

        public override async Task<CommandResult> ProceedCallbackAsync(CallbackQuery eCallbackQuery, Player player, CommandResult commandResult)
        {
            switch (eCallbackQuery.Data)
            {
                case "/meadow_search":
                    player.Money += 30;
                    player.Exp += 10;
                    commandResult.Text.AppendLine("Деньги +30, опыт+10");
                    commandResult.Text.AppendLine("Да-да, ходил по поляне и нашел деньги...");
                    commandResult.Text.AppendLine("Когда-нибудь тут будут только материалы");
                    break;
                case "/forest_search":
                    player.Money += 30;
                    player.Exp += 10;
                    commandResult.Text.AppendLine("Деньги +30, опыт+10");
                    commandResult.Text.AppendLine("Да-да, ходил по краю леса и нашел деньги...");
                    commandResult.Text.AppendLine("Когда-нибудь тут будут более крутые материалы");
                    break;
                case "/forest_deep_search":
                    player.Money += 30;
                    player.Exp += 10;
                    commandResult.Text.AppendLine("Деньги +30, опыт+10");
                    commandResult.Text.AppendLine("Да-да, ходил в лес и нашел деньги...");
                    commandResult.Text.AppendLine("Когда-нибудь тут будут не только материалы, а и разные приключения + предметы");
                    break;
            }

            return commandResult;
        }
    }
}