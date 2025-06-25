using Chess.Models;
using System.Threading.Tasks;

public interface IGameService
{
    Game InitializeGame(int numberOfPlayers);
    Task<Game?> MarkPossibleMovesAsync(string? userId, int pieceId);
}