using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Telegram.Bot.Types;

namespace GameLibrary
{
    public class Database
    {
        private IMongoCollection<Player> Players;
        private IMongoCollection<BsonDocument> Logs;
        public Database(string mongoConnectionString)
        {
            var client = new MongoClient(mongoConnectionString).GetDatabase("TelegramBotGame");
            Players = client.GetCollection<Player>("Players");
            Logs = client.GetCollection<BsonDocument>("Logs");
        }

        public Player GetPlayer(User user)
        {
            var filter = Builders<Player>.Filter.Eq(x=>x.Id, user.Id);
            return Players.Find(filter).FirstOrDefault();
        }
        public Player AddPlayer(User user)
        {
            var player = new Player(user.Id);
            Players.InsertOne(player);
            return player;
        }

        public async Task<Player> UpdatePlayer(Player pl, CancellationToken cancellationToken = default)
        {
            return await Players.FindOneAndReplaceAsync(p=>p.Id == pl.Id, pl, cancellationToken: cancellationToken);
        }

        public async Task LogMessage(Message mess, CancellationToken cancellationToken = default)
        {
             await Logs.InsertOneAsync(mess.ToBsonDocument(), null, cancellationToken);
        }

        public void LogMessage(CallbackQuery que)
        {
            Logs.InsertOneAsync(que.ToBsonDocument());
        }
    }
}