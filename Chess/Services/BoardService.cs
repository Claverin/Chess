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
            return await _mongoDbService.GetGamesCollection()
                .Find(g => g.OwnerId == userId && g.IsGameActive)
                .FirstOrDefaultAsync();
        }
    }
}