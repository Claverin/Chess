using Chess.Domain.Entities;
using MongoDB.Bson;

namespace Chess.Interfaces.Repository
{
    public interface IGameRepository
    {
    Task<Game?> GetActive(ObjectId ownerId);
    Task<Game?> GetLast(ObjectId ownerId);

    Task Insert(Game game);
    Task Save(Game game);

    Task DeactivateActive(ObjectId ownerId);
    }
}