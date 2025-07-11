using Chess.Data;
using Chess.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Chess.Services
{
    public class BoardService
    {
        private readonly MongoDbService _mongoDbService;

        public BoardService(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public async Task<Game> SearchForGameAsync(ObjectId userId)
        {
            var gamesCollection = _mongoDbService.GetGamesCollection();

            return await gamesCollection
                .Find(g => g.IsGameActive && g.Players.Any(p => p.UserId == userId))
                .FirstOrDefaultAsync();
        }
    }
}