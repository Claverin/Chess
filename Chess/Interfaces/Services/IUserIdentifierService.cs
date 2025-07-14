using MongoDB.Bson;

namespace Chess.Abstractions.Services
{
    public interface IUserIdentifierService
    {
        ObjectId GetUserObjectId();
    }
}
