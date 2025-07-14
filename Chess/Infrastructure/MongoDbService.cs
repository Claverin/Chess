using Chess.Intefaces.Infrastructure;
using Chess.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Chess.Infrastructure
{
    public class MongoDbService : IMongoDbService
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<Game> _gamesCollection;

        public MongoDbService(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
            _database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);

            try
            {
                IsServerAlive();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("IsServerAlive unhandled exception", ex);
            }

            _gamesCollection = _database.GetCollection<Game>("Games");
        }

        public IMongoCollection<Game> GetGamesCollection()
        {
            return _gamesCollection;
        }

        public void IsServerAlive()
        {
            var command = new BsonDocument { { "ping", 1 } };
            _database.RunCommand<BsonDocument>(command);
        }
    }
}