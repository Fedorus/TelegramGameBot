using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace GameLibrary
{
    public class CommandResult
    {
        public StringBuilder Text { get; } = new StringBuilder();
        public List<InlineKeyboardButton> Buttons { get; } = new List<InlineKeyboardButton>();
        public bool BlockResult { get; set; }
        public MessageType Type { get; set; } = MessageType.Text;
        public object AdditionalData { get; set; }
    }
}