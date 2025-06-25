using Chess.Data;
using Chess.Models;
using MongoDB.Driver;

namespace Chess.Services
{
    public class GameService : IGameService
    {
        private readonly MongoDbService _mongoDbService;
        private readonly GameSetupService _gameSetupService;
        private readonly BoardService _boardService;
        private readonly MovementPieceService _movementPieceService;

        public GameService(MongoDbService mongoDbService, GameSetupService gameSetupService, BoardService boardService, MovementPieceService movementPieceService)
        {
            _mongoDbService = mongoDbService;
            _gameSetupService = gameSetupService;
            _boardService = boardService;
            _movementPieceService = movementPieceService;
        }

        public Game InitializeGame(int numberOfPlayers)
        {
            return _gameSetupService.SetupNewGame(numberOfPlayers);
        }

        public async Task<Game?> MarkPossibleMovesAsync(string? userId, int pieceId)
        {
            Game game = await _boardService.SearchForGameAsync(userId);

            game = _movementPieceService.SelectPieceAndHighlightMoves(game, pieceId);

            await _mongoDbService.GetGamesCollection().ReplaceOneAsync(g => g.Id == game.Id, game);

            return game;
        }
    }
}