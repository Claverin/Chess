using Chess.Domain.Entities;
using MongoDB.Driver;

namespace Chess.Interfaces.Infrastructure
{
    public interface IMongoDbService
    {
        void IsServerAlive();
        IMongoCollection<Game> GetGamesCollection();
    }
}