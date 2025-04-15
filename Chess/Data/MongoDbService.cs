using Chess.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Chess.Data
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<Game> _gamesCollection;

        public MongoDbService(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
            _database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);

            // Sprawdzenie połączenia
            try
            {
                _database.RunCommand((Command<BsonDocument>)"{ping:1}");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Nie udało się połączyć z bazą MongoDB.", ex);
            }

            _gamesCollection = _database.GetCollection<Game>("Games");
        }

        // Zwróć kolekcję gier
        public IMongoCollection<Game> GetGamesCollection()
        {
            return _gamesCollection;
        }
    }
}