using System.Threading.Tasks;
using GameLibrary.Helpers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace GameLibrary.Controllers
{
    public class Help : Controller
    {
        private readonly TelegramBotClient _client;

        public Help(TelegramBotClient client)
        {
            _client = client;
        }

        public override bool CheckMessage(Message eMessage, Player player)
        {
            return eMessage.Text == "/help";
        }

        public override async Task<CommandResult> ProceedMessageAsync(Message eMessage, Player player, CommandResult commandResult)
        {
            player.RefreshKeyboard(_client);
            return commandResult;
        }

        public override bool CheckCallback(CallbackQuery eCallbackQuery, Player player)
        {
            return false;
        }

        public override async Task<CommandResult> ProceedCallbackAsync(CallbackQuery eCallbackQuery, Player player, CommandResult commandResult)
        {
            return commandResult;
        }
    }
}