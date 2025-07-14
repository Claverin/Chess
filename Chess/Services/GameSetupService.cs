using Chess.Domain.Entities;

namespace Chess.Services
{
    public class GameSetupService
    {
        private readonly BoardSetupService _boardSetupService;
        private readonly PieceSetupService _setupGamePieceService;
        public GameSetupService(BoardSetupService boardSetupService, PieceSetupService setupGamePieceService)
        {
            _boardSetupService = boardSetupService;
            _setupGamePieceService = setupGamePieceService;
        }
        public Game SetupNewGame(int numberOfPlayers)
        {
            try
            {
                var game = _boardSetupService.CreateNewBoardWithRules(numberOfPlayers);
                game.Board = _setupGamePieceService.PutPiecesOnBoard(game.Board);
                return game;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("SetupNewGame Error", ex);
            }
        }
    }
}