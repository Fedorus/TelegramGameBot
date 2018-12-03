using System.Globalization;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace GameLibrary.Controllers
{
    public class Items : Controller
    {
        public override bool CheckMessage(Message eMessage, Player player)
        {
            return eMessage.Text != null && (eMessage.Text.StartsWith("/item_"));
        }

        public override bool CheckCallback(CallbackQuery eCallbackQuery, Player player)
        {
            return (eCallbackQuery.Data.StartsWith("/item_"));
        }

        public override async Task<CommandResult> ProceedMessageAsync(Message eMessage, Player player, CommandResult commandResult)
        {
            Process(eMessage.Text, player, commandResult);
            return commandResult;
        }

        public override async Task<CommandResult> ProceedCallbackAsync(CallbackQuery eCallbackQuery, Player player, CommandResult commandResult)
        {
            Process(eCallbackQuery.Data, player, commandResult);
            return commandResult;
        }

        private void Process(string request, Player player, CommandResult commandResult)
        {
            int id = -1;
            if (request.StartsWith("/item_details_"))
            {
                id = int.Parse(request.Substring("/item_details_".Length).Trim());
                commandResult.Text.AppendLine($"<b>{player.Items[id].Name}</b>");
                commandResult.Text.AppendLine($"<code>{player.Items[id].Description}</code>");
                if (!string.IsNullOrWhiteSpace(player.Items[id].ImageLink))
                {
                    commandResult.Type = MessageType.Photo;
                    commandResult.AdditionalData = player.Items[id].ImageLink;
                }
                if (!player.Items[id].CanBeSold)
                {
                    commandResult.Text.AppendLine("Нельзя продать");
                }
                else
                {
                    commandResult.Text.AppendLine($"Цена: {player.Items[id].Cost.ToString(CultureInfo.InvariantCulture)}");
                }
            }
            else if (request.StartsWith("/item_sell_"))
            {
                id= int.Parse(request.Substring("/item_sell_".Length).Trim());
                if (player.Lvl <5)
                {
                    commandResult.Text.AppendLine("Пока нельзя продать. Уничтожить?");
                    commandResult.Buttons.Add(new InlineKeyboardButton(){ Text = "Уничтожить", CallbackData = $"/item_delete_{id.ToString()}"});
                }
                else
                {
                    player.Money += player.Items[id].Cost;
                    commandResult.Text.AppendLine($"Успешно продано за: {player.Items[id].Cost.ToString(CultureInfo.InvariantCulture)}");
                    player.Items.Remove(player.Items[id]);
                }
                return;
            }
            else if (request.StartsWith("/item_delete_"))
            {
                id = int.Parse((request.Substring("/item_delete_".Length)).Trim());
                player.Items.Remove(player.Items[id]);
                commandResult.Text.AppendLine("Предмет удалён");
                return;
                
            }

            if (id>=0 && player.Items[id].CanBeSold)
            {
                commandResult.Buttons.Add(new InlineKeyboardButton(){ Text = "Продать", CallbackData =
                    $"/item_sell_{id.ToString()}"
                });
            }
            else if(id>=0)
            {
                commandResult.Buttons.Add(new InlineKeyboardButton(){ Text = "Уничтожить", CallbackData = $"/item_delete_{id.ToString()}"});
            }
            else
            {
                commandResult.Text.AppendLine("Предмет не найден");
            }
        }
    }
}