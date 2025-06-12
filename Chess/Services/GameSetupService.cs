using Chess.Models;

namespace Chess.Services
{
    public class GameSetupService
    {
        private readonly GameBoarSetupService _gameBoarSetupService;
        private readonly PieceSetupService _setupGamePieceService;
        public GameSetupService(GameBoarSetupService gameBoarSetupService, PieceSetupService setupGamePieceService)
        {
            _gameBoarSetupService = gameBoarSetupService;
            _setupGamePieceService = setupGamePieceService;
        }
        public Game SetupNewGame(int numberOfPlayers)
        {
            try
            {
                var game = _gameBoarSetupService.CreateNewGame(numberOfPlayers);
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