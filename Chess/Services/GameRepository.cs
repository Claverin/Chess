using Chess.Domain.Entities;
using Chess.Interfaces.Infrastructure;
using Chess.Interfaces.Repository;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Chess.Services
{
    public class GameRepository : IGameRepository
    {
        private readonly IMongoCollection<Game> _games;

        public GameRepository(IMongoDbService mongoDbService)
        {
            _games = mongoDbService.GetGamesCollection();
        }

        public Task<Game?> GetActive(ObjectId ownerId)
        {
            return _games.Find(g => g.OwnerId == ownerId && g.IsGameActive).FirstOrDefaultAsync();
        }

        public Task<Game?> GetLast(ObjectId ownerId)
        {
            return _games.Find(g => g.OwnerId == ownerId).SortByDescending(g => g.Id).FirstOrDefaultAsync();
        }
        public Task Insert(Game game)
        {
            return _games.InsertOneAsync(game);
        }

        public Task Save(Game game)
        {
            return _games.ReplaceOneAsync(g => g.Id == game.Id, game);
        }
        public Task DeactivateActive(ObjectId ownerId)
        {
            return _games.UpdateManyAsync(g => g.OwnerId == ownerId && g.IsGameActive,
                Builders<Game>.Update.Set(g => g.IsGameActive, false));
        }
    }
}
