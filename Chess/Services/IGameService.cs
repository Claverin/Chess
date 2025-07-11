using Chess.Models;
using MongoDB.Bson;
using System.Threading.Tasks;

public interface IGameService
{
    Game InitializeGame(int numberOfPlayers);
    Task<Game> MarkPossibleMoves(ObjectId userId, int pieceId);
    Task<Game> TryMovePieceAsync(ObjectId userId, int x, int y);
}