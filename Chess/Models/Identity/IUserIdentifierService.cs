using MongoDB.Bson;

namespace Chess.Models.Identity
{
    public interface IUserIdentifierService
    {
        ObjectId CreateOrGetUserObjectId();
    }
}
