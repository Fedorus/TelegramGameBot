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
            var client = new MongoClient(mongoConnectionString).GetDatabase("Game");
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

        public Player UpdatePlayer(Player pl)
        {
            return Players.FindOneAndReplace(p=>p.Id == pl.Id, pl);
        }

        public void LogMessage(Message mess)
        {
             Logs.InsertOneAsync(mess.ToBsonDocument());
        }

        public void LogMessage(CallbackQuery que)
        {
            Logs.InsertOneAsync(que.ToBsonDocument());
        }
    }
}