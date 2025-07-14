using Chess.Domain.Entities;
using MongoDB.Driver;

namespace Chess.Intefaces.Infrastructure
{
    public interface IMongoDbService
    {
        void IsServerAlive();
        IMongoCollection<Game> GetGamesCollection();
    }
}