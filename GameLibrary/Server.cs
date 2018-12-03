using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GameLibrary.Helpers;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using File = Telegram.Bot.Types.File;

namespace GameLibrary
{
    public class Server
    {
        private readonly TelegramBotClient _client;
        private readonly List<Controller> _controllers = new List<Controller>();
        private readonly Database _db;
        public Server(TelegramBotClient client, string MongoConnectionString)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _db = new Database(MongoConnectionString);
        }

        public Server Add(params Controller[] controllers)
        {
            _controllers.AddRange(controllers);
            return this;
        }

        private void Subscribe()
        {
            _client.OnMessage += ClientOnOnMessage;
            _client.OnCallbackQuery += ClientOnOnCallbackQuery;
            _client.OnInlineQuery += ClientOnOnInlineQuery;
            _client.OnReceiveError += ClientOnOnReceiveError;
            _client.OnInlineResultChosen += ClientOnOnInlineResultChosen;
            _client.OnMessageEdited += ClientOnOnMessageEdited;
        }

        private void ClientOnOnMessageEdited(object sender, MessageEventArgs e)
        {
            Console.WriteLine(e.Message.Text);
        }

        private void ClientOnOnInlineResultChosen(object sender, ChosenInlineResultEventArgs e)
        {
            Console.WriteLine(e.ChosenInlineResult);
        }

        private void ClientOnOnReceiveError(object sender, ReceiveErrorEventArgs e)
        {
            Console.WriteLine(e.ApiRequestException);
        }

        private void ClientOnOnInlineQuery(object sender, InlineQueryEventArgs e)
        {
            Console.WriteLine(e.InlineQuery);
        }

        private async void ClientOnOnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            _db.LogMessage(e.CallbackQuery);
            var player = await GetPlayer( e.CallbackQuery.From);
            Console.WriteLine(e.CallbackQuery.Data);
            var commandResult = new CommandResult();
            foreach (var command in _controllers)
            {
                if (command.CheckCallback(e.CallbackQuery, player))
                {
                    await command.ProceedCallbackAsync(e.CallbackQuery, player, commandResult);
                }
            }
            _db.UpdatePlayer(player);
            HandleResponse(commandResult, e.CallbackQuery.From.Id);
        }

        private async void ClientOnOnMessage(object sender, MessageEventArgs e)
        {
            _db.LogMessage(e.Message);
            var player = await GetPlayer(e.Message.From);
            Console.WriteLine(e.Message.Text);
            var commandResult = new CommandResult();
            foreach (var command in _controllers)
            {
                if (command.CheckMessage(e.Message, player))
                {
                   await command.ProceedMessageAsync(e.Message, player, commandResult);
                }
            }
            _db.UpdatePlayer(player);
            HandleResponse(commandResult, e.Message.From.Id);
        }

        private async Task HandleResponse(CommandResult commandResult, int chat)
        {
            switch (commandResult.Type)
            {
                 case MessageType.Photo:
                     if (commandResult.AdditionalData is string)
                     {
                         await _client.SendPhotoAsync(chat, new InputOnlineFile((string) commandResult.AdditionalData),
                             commandResult.Text.ToString(), ParseMode.Html,
                             replyMarkup: new InlineKeyboardMarkup(commandResult.Buttons.SplitList()));
                     }
                     else if (commandResult.AdditionalData is Stream)
                     {
                         await _client.SendPhotoAsync(chat, new InputOnlineFile((Stream) commandResult.AdditionalData),
                             commandResult.Text.ToString(), ParseMode.Html,
                             replyMarkup: new InlineKeyboardMarkup(commandResult.Buttons.SplitList()));
                     }
                     break;
                 case MessageType.Text:
                     default:
                     await _client.SendTextMessageAsync(chat, commandResult.Text.ToString(),
                         replyMarkup: new InlineKeyboardMarkup(commandResult.Buttons.SplitList()), parseMode: ParseMode.Html);
                     break;
                     
            }
        }

        private async Task<Player> RegisterPlayer(User messageFrom)
        {
            var player = _db.AddPlayer(messageFrom);
            Console.WriteLine($"Registered: {player.Id.ToString()}");
            await _client.SendTextMessageAsync(messageFrom.Id, "You have successfully registered!");
            return player;
        }

        private async Task<Player> GetPlayer(User user)
        {
            var player = _db.GetPlayer(user) ?? await RegisterPlayer(user);
            player.OnLvlUp += PlayerOnOnLvlUp;
            return player;
        }

        private void PlayerOnOnLvlUp(Player player)
        {
            //using (var file = System.IO.File.OpenRead("Photo/lvlup.png"))
            {
                this._client.SendPhotoAsync(player.Id,
                    new InputMedia(
                        "https://vignette.wikia.nocookie.net/roblox/images/9/9b/Level_Up_Logo.png/revision/latest?cb=20171101220707"),
                    caption: $"LVL UP! Новый уровень - {player.Lvl.ToString()}!");
            }
        }

        public Server Start()
        {
            Subscribe();
            _client.StartReceiving();
            Console.WriteLine("Started!");
            return this;
        }
    }
}