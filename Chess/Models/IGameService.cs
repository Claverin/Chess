using Chess.Models;
using System.Threading.Tasks;

public interface IGameService
{
    Game InitializeGame(int numberOfPlayers);
    Task<Game?> MakeMoveAsync(string? userId, string moveNotation);
    void RecordMove(Game game, string moveNotation);
    void ReconstructBoardFromMoves(Game game);
    Game SelectPieceAndHighlightMoves(Game game, int pieceId);
    bool IsPathClear(Game game, Cell fromCell, Cell toCell);
    bool IsMoveLegal(Piece piece, (int x, int y) target);
}