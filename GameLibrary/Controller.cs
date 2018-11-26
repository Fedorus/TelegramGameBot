using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace GameLibrary
{
    public class Controller
    {
        public bool CheckMessage(Message eMessage, Player player)
        {
            return false;
        }

        public async Task<CommandResult> ProceedMessageAsync(Message eMessage, Player player, CommandResult commandResult)
        {
            return commandResult;
        }

        public bool CheckCallback(CallbackQuery eCallbackQuery, Player player)
        {
            return false;
        }

        public async Task<CommandResult> ProceedCallbackAsync(CallbackQuery eCallbackQuery, Player player, CommandResult commandResult)
        {
            return commandResult;
        }
    }
}