using MongoDB.Bson;
using MongoDB.Driver;

namespace GameLibrary
{
    public class Database
    {
        public Database(string mongoConnectionString)
        {
            var client = new MongoClient(mongoConnectionString).GetDatabase("Game");
            Players = client.GetCollection<BsonDocument>("Players");
        }

        private IMongoCollection<BsonDocument> Players;
            
        public Player GetPlayer(int id)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("id", id);
            return Players.Find(filter).As<Player>().FirstOrDefault();
        }

        public Player AddPlayer(int id)
        {
            var player = new Player(id);
            Players.InsertOne(player.ToBsonDocument());
            return player;
        }
    }
}