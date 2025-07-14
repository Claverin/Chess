using MongoDB.Bson;

public interface IGameTrackerService
{
    void SetCurrentGameId(ObjectId gameId);
    ObjectId? GetCurrentGameId();
    void ClearCurrentGameId();
}