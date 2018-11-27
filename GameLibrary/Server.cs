using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

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

        public Server AddController(params Controller[] controllers)
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
            var player = _db.GetPlayer(e.CallbackQuery.Message.From.Id) ?? await RegisterPlayer(e.CallbackQuery.From);
            Console.WriteLine(e.CallbackQuery.Data);
            var commandResult = new CommandResult();
            foreach (var command in _controllers)
            {
                if (command.CheckCallback(e.CallbackQuery, player))
                {
                    await command.ProceedCallbackAsync(e.CallbackQuery, player, commandResult);
                }
            }
        }

        private async void ClientOnOnMessage(object sender, MessageEventArgs e)
        {
            var player =  _db.GetPlayer(e.Message.From.Id) ?? await RegisterPlayer(e.Message.From);
            Console.WriteLine(e.Message.Text);
            var commandResult = new CommandResult();
            foreach (var command in _controllers)
            {
                if (command.CheckMessage(e.Message, player))
                {
                   await command.ProceedMessageAsync(e.Message, player, commandResult);
                }
            }
        }

        private async Task<Player> RegisterPlayer(User messageFrom)
        {
            var player = _db.AddPlayer(messageFrom.Id);
            Console.WriteLine($"Registered: {player.Id.ToString()}");
            await _client.SendTextMessageAsync(messageFrom.Id, "You have successfully registered!");
            return player;
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