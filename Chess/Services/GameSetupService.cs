using Chess.Domain.Entities;
using MongoDB.Bson;

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

        public Game CreateNewGame(ObjectId userId, int numberOfPlayers)
        {
            var game = SetupNewGame(numberOfPlayers);
            game.OwnerId = userId;
            game.IsGameActive = true;

            return game;
        }

        private Game SetupNewGame(int numberOfPlayers)
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