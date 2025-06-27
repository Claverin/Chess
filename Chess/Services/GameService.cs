using Chess.Data;
using Chess.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;

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

        public async Task<Game> MarkPossibleMovesAsync(ObjectId userId, int pieceId)
        {
            Game game = await _boardService.SearchForGameAsync(userId);
            game = _movementPieceService.SelectPieceAndHighlightMoves(game, pieceId);
            await _mongoDbService.GetGamesCollection().ReplaceOneAsync(g => g.Id == game.Id, game);
            return game;
        }

        public async Task<Game> TryMovePieceAsync(ObjectId userId, Field field)
        {
            var game = await _boardService.SearchForGameAsync(userId);
            if (game == null || game.ActivePieceId == null || game.AvailableMoves == null)
                return null;

            var piece = game.Board.Pieces.FirstOrDefault(p => p.Id == game.ActivePieceId);
            if (piece == null)
                return game;

            var selectedField = game.Board.FindCellByCoordinates(field.X, field.Y);

            if (game.AvailableMoves.Any(f => f.X == field.X && f.Y == field.Y))
            {
                piece.CurrentPosition = selectedField.Field;
                selectedField.Piece = piece;
                game.ActivePieceId = null;
                game.AvailableMoves.Clear();
            }

            await _mongoDbService.GetGamesCollection().ReplaceOneAsync(g => g.Id == game.Id, game);
            return game;
        }
    }
}