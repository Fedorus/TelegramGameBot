using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace GameLibrary
{
    public class CommandResult
    {
        public StringBuilder Text { get; }
        public List<IKeyboardButton> Buttons { get; }
        public bool BlockResult { get; set; }
    }
}