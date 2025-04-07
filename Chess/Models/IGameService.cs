using Chess.Models;

public interface IGameService
{
    Game InitializeGame(int numberOfPlayers);
    Game SelectPieceAndHighlightMoves(Game game, int pieceId);
    bool IsPathClear(Game game, Cell fromCell, Cell toCell);
}