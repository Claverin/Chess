using Chess.Domain.Entities;
using MongoDB.Bson;

public interface IGameService
{
    Game InitializeGame(int numberOfPlayers);
    Task<Game> MarkPossibleMoves(ObjectId userId, int pieceId);
    Task<Game> TryMovePieceAsync(ObjectId userId, int x, int y);
    void MarkPiecesWithLegalMoves(Game game);
}