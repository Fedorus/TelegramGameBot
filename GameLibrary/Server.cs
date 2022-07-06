using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using GameLibrary.Helpers;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace GameLibrary
{
    public class Server
    {
        private readonly ITelegramBotClient _client;
        private readonly List<Controller> _controllers = new List<Controller>();
        private readonly Database _db;
        public Server(ITelegramBotClient client, string MongoConnectionString)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _db = new Database(MongoConnectionString);
        }

        public Server Add(params Controller[] controllers)
        {
            _controllers.AddRange(controllers);
            return this;
        }

        private async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
        {
            switch (update.Type)
            {
                case UpdateType.Unknown:
                    break;
                case UpdateType.Message:
                    await OnMessage(update.Message, cancellationToken);
                    break;
                case UpdateType.InlineQuery:
                    Console.WriteLine(update.InlineQuery?.Query);
                    break;
                case UpdateType.ChosenInlineResult:
                    Console.WriteLine(update.ChosenInlineResult?.Query);
                    break;
                case UpdateType.CallbackQuery:
                    await OnCallbackQuery(update.CallbackQuery, cancellationToken);
                    break;
                case UpdateType.EditedMessage:
                    Console.WriteLine(update.Message?.Text);
                    break;
                case UpdateType.ChannelPost:
                    break;
                case UpdateType.EditedChannelPost:
                    break;
                case UpdateType.ShippingQuery:
                    break;
                case UpdateType.PreCheckoutQuery:
                    break;
                case UpdateType.Poll:
                    break;
                case UpdateType.PollAnswer:
                    break;
                case UpdateType.MyChatMember:
                    break;
                case UpdateType.ChatMember:
                    break;
                case UpdateType.ChatJoinRequest:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private static Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException =>
                    $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(errorMessage);
            Console.ResetColor();
            
            return Task.CompletedTask;
        }

        private async Task OnCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            if (callbackQuery == null)
                return;
            
            _db.LogMessage(callbackQuery);
            var player = await GetPlayer(callbackQuery.From);
            Console.WriteLine(callbackQuery.Data);
            var commandResult = new CommandResult();
            foreach (var command in _controllers)
            {
                if (command.CheckCallback(callbackQuery, player))
                {
                    await command.ProceedCallbackAsync(callbackQuery, player, commandResult);
                }
            }
            await _db.UpdatePlayer(player, cancellationToken);
            await HandleResponse(commandResult, callbackQuery.From.Id, cancellationToken);
        }

        private async Task OnMessage(Message message, CancellationToken cancellationToken)
        {
            if (message?.From == null)
                return;
            
            await _db.LogMessage(message, cancellationToken);
            var player = await GetPlayer(message.From);
            Console.WriteLine(message.Text);
            var commandResult = new CommandResult();
            foreach (var command in _controllers)
            {
                if (command.CheckMessage(message, player))
                {
                   await command.ProceedMessageAsync(message, player, commandResult);
                }
            }
            
            await _db.UpdatePlayer(player, cancellationToken);
            await HandleResponse(commandResult, message.From.Id, cancellationToken);
        }

        private async Task HandleResponse(CommandResult commandResult, long chat, CancellationToken cancellationToken)
        {
            switch (commandResult.Type)
            {
                case MessageType.Photo:
                    if (commandResult.AdditionalData is string)
                    {
                        await _client.SendPhotoAsync(chat, new InputOnlineFile((string) commandResult.AdditionalData),
                            commandResult.Text.ToString(), ParseMode.Html,
                            replyMarkup: new InlineKeyboardMarkup(commandResult.Buttons.SplitList()),
                            cancellationToken: cancellationToken);
                    }
                    else if (commandResult.AdditionalData is Stream)
                    {
                        await _client.SendPhotoAsync(chat, new InputOnlineFile((Stream) commandResult.AdditionalData),
                            commandResult.Text.ToString(), ParseMode.Html,
                            replyMarkup: new InlineKeyboardMarkup(commandResult.Buttons.SplitList()),
                            cancellationToken: cancellationToken);
                    }

                    break;
                case MessageType.Text:
                default:
                    await _client.SendTextMessageAsync(chat, commandResult.Text.ToString(),
                        replyMarkup: new InlineKeyboardMarkup(commandResult.Buttons.SplitList()),
                        parseMode: ParseMode.Html, cancellationToken: cancellationToken);
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
                _client.SendPhotoAsync(player.Id,
                    new InputMedia(
                        "https://vignette.wikia.nocookie.net/roblox/images/9/9b/Level_Up_Logo.png/revision/latest?cb=20171101220707"),
                    caption: $"LVL UP! Новый уровень - {player.Lvl.ToString()}!");
            }
        }

        public Server Start()
        {
            using var cts = new CancellationTokenSource();
            
            var receiverOptions = new ReceiverOptions
            {
                // receive all update types
                AllowedUpdates = Array.Empty<UpdateType>() 
            };
            
            _client.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token);
            
            Console.WriteLine("Started!");
            return this;
        }
    }
}