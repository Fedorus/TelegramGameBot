using System;
using System.Globalization;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace GameLibrary.Controllers
{
    public class Bag : Controller
    {
        public override bool CheckMessage(Message eMessage, Player player)
        {
            return eMessage.Text == "/bag";
        }

        public override bool CheckCallback(CallbackQuery eCallbackQuery, Player player)
        {
            return false;
        }

        public override async Task<CommandResult> ProceedMessageAsync(Message eMessage, Player player, CommandResult commandResult)
        {
            var i = 0;
            commandResult.Text.AppendLine($"Предметы в инвентаре:");
            foreach (var item in player.Items)
            {
                commandResult.Text.Append($"[{i.ToString()}]");
                switch (item.Type)
                {
                    case ItemType.Weapon:
                        commandResult.Text.Append("🗡");
                        break;
                    case ItemType.Armor:
                        commandResult.Text.Append("🛡");
                        break;
                    case ItemType.QuestItem:
                        commandResult.Text.Append("📜");
                        break;
                    case ItemType.CraftShit:
                        commandResult.Text.Append("🔧");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                    commandResult.Text.AppendLine($"\"{item.Name}\" Цена: {item.Cost.ToString(CultureInfo.InvariantCulture)} /item_details_{i.ToString()}");
                i++;
            }
            //commandResult.Buttons.Add(new InlineKeyboardButton(){ Text = ""});
            return commandResult;
        }

        public override async Task<CommandResult> ProceedCallbackAsync(CallbackQuery eCallbackQuery, Player player, CommandResult commandResult)
        {
            return commandResult;
        }
    }
}