using Chess.Domain.Entities;
using Chess.Domain.Entities.Pieces;
using MongoDB.Bson.Serialization;

namespace Chess.Infrastructure
{
    public static class MongoDbMappings
    {
        public static void RegisterClassMaps()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(Piece)))
            {
                BsonClassMap.RegisterClassMap<Piece>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIsRootClass(true);
                });
            }

            foreach (var pieceType in new[]
            {
            typeof(Pawn), typeof(Rook), typeof(Knight),
            typeof(Bishop), typeof(Queen), typeof(King)
        })
            {
                if (!BsonClassMap.IsClassMapRegistered(pieceType))
                {
                    BsonClassMap.RegisterClassMap(new BsonClassMap(pieceType));
                }
            }
        }
    }
}