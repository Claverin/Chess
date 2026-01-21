using Chess.Domain.Entities;

namespace Chess.Abstractions.Services
{
    public interface IGameService
    {
        Task<Game?> GetCurrentGame();
        Task<Game> InitializeGame(int numberOfPlayers);
        Task<Game> CreateNewGame(int numberOfPlayers);
        Task<Game?> SelectPiece(int pieceId);
        Task<Game?> MovePiece(int x, int y);
        void MarkPiecesWithLegalMoves(Game game);
    }
}
