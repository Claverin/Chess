using MongoDB.Bson;

namespace Chess.Interfaces.Services
{
    public interface IUserIdentifierService
    {
        ObjectId GetUserObjectId();
    }
}
