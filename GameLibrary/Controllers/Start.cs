using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace GameLibrary.Controllers
{
    public class Start : Controller
    {
        public override bool CheckMessage(Message eMessage, Player player)
        {
            return eMessage.Text !=null && eMessage.Text.StartsWith("/start");
        }

        public async override Task<CommandResult> ProceedMessageAsync(Message eMessage, Player player, CommandResult commandResult)
        {
            commandResult.Text.AppendLine("Вы очутились на поляне посреди леса. Почему???");
            commandResult.Buttons.Add(new InlineKeyboardButton(){ Text = "Home", CallbackData = "/home"});
            return commandResult;
        }

        public override bool CheckCallback(CallbackQuery eCallbackQuery, Player player)
        {
            return false;
        }

        public async override Task<CommandResult> ProceedCallbackAsync(CallbackQuery eCallbackQuery, Player player, CommandResult commandResult)
        {
            return commandResult;
        }
    }
}