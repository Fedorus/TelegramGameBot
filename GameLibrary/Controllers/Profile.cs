using System;
using System.Globalization;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace GameLibrary.Controllers
{
    public class Profile : Controller
    {
        public override bool CheckMessage(Message eMessage, Player player)
        {
            return eMessage.Text == "👤Профиль";
        }        
        
        public override bool CheckCallback(CallbackQuery eCallbackQuery, Player player)
        {
            return false;
        }

        public override async Task<CommandResult> ProceedMessageAsync(Message eMessage, Player player, CommandResult commandResult)
        {
            CommandProcess(player, commandResult);
            return commandResult;
        }
        public override async Task<CommandResult> ProceedCallbackAsync(CallbackQuery eCallbackQuery, Player player, CommandResult commandResult)
        {
            return commandResult;
        }

        private void CommandProcess(Player player, CommandResult commandResult)
        {
            commandResult.Text.AppendLine($"<code>id: {player.Id.ToString()}</code>");
            commandResult.Text.AppendLine($"🔶Уровень: {player.Lvl.ToString()}");
            commandResult.Text.AppendLine($"🏵Опыт: {player.Exp.ToString()}/{Player.LvlCap[player.Lvl].ToString()}");
            commandResult.Text.AppendLine($"💰Деньги: {player.Money.ToString(CultureInfo.InvariantCulture)}");
            commandResult.Text.AppendLine($"📦Рюкзак: {player.Items.Count.ToString()}/{player.Items.Size.ToString()} /bag");
            //commandResult.Text.AppendLine($"Donate: {player.Donate.ToString()}");
        }
    }
}