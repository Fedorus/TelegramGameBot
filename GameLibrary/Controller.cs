using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace GameLibrary
{
    public abstract class Controller
    {
        public abstract bool CheckMessage(Message eMessage, Player player);
        public abstract bool CheckCallback(CallbackQuery eCallbackQuery, Player player);
        
        public abstract Task<CommandResult> ProceedMessageAsync(Message eMessage, Player player,
            CommandResult commandResult);
        public abstract Task<CommandResult> ProceedCallbackAsync(CallbackQuery eCallbackQuery, Player player,
            CommandResult commandResult);
    }
}