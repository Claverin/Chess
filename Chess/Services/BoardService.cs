using Chess.Domain.Entities;
using Chess.Infrastructure;
using Chess.Intefaces.Infrastructure;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Chess.Services
{
    public class BoardService
    {
        private readonly IMongoDbService _mongoDbService;

        public BoardService(IMongoDbService mongoDbService)
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