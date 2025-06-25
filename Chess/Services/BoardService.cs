using Chess.Data;
using Chess.Models;
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

        public async Task<Game> SearchForGameAsync(string userId)
        {
            var gamesCollection = _mongoDbService.GetGamesCollection();
            var queryId = string.IsNullOrEmpty(userId) ? "guest" : userId;

            return await gamesCollection
                .Find(g => g.Active && g.Players.Any(p => p.UserId.ToString() == queryId))
                .FirstOrDefaultAsync();
        }
    }
}