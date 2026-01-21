using Chess.Abstractions.Services;
using Chess.Domain.Entities;
using Chess.Domain.Enums;
using Chess.Intefaces.Infrastructure;
using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace Chess.Services
{
    public class GameService : IGameService
    {
        private readonly IMongoDbService _mongoDbService;
        private readonly IUserIdentifierService _userIdentifierService;

        private readonly GameSetupService _gameSetupService;
        private readonly BoardService _boardService;
        private readonly MovementPieceService _movementPieceService;
        private readonly IGameRulesService _rulesService;

        public GameService(
            IMongoDbService mongoDbService,
            IUserIdentifierService userIdentifierService,
            GameSetupService gameSetupService,
            BoardService boardService,
            MovementPieceService movementPieceService,
            IGameRulesService rulesService)
        {
            _mongoDbService = mongoDbService;
            _userIdentifierService = userIdentifierService;
            _gameSetupService = gameSetupService;
            _boardService = boardService;
            _movementPieceService = movementPieceService;
            _rulesService = rulesService;
        }

        public async Task<Game?> GetCurrentGame()
        {
            var userId = RequireUserId();
            var game = _mongoDbService.GetGamesCollection();

            var active = await game.Find(g => g.OwnerId == userId && g.IsGameActive).FirstOrDefaultAsync();

            if (active != null) return active;

            var last = await game.Find(g => g.OwnerId == userId).SortByDescending(g => g.Id).FirstOrDefaultAsync();

            return last;
        }

        public async Task<Game> InitializeGame(int numberOfPlayers)
        {
            var userId = RequireUserId();
            var games = _mongoDbService.GetGamesCollection();

            var existing = await games
                .Find(g => g.OwnerId == userId && g.IsGameActive)
                .FirstOrDefaultAsync();

            if (existing != null)
                return existing;

            var game = _gameSetupService.CreateNewGame(userId, numberOfPlayers);
            await games.InsertOneAsync(game);
            return game;
        }

        public async Task<Game> CreateNewGame(int numberOfPlayers)
        {
            var userId = RequireUserId();
            var games = _mongoDbService.GetGamesCollection();

            await games.UpdateManyAsync(
                g => g.OwnerId == userId && g.IsGameActive,
                Builders<Game>.Update.Set(g => g.IsGameActive, false)
            );

            var game = _gameSetupService.CreateNewGame(userId, numberOfPlayers);
            await games.InsertOneAsync(game);
            return game;
        }

        public async Task<Game?> SelectPiece(int pieceId)
        {
            var userId = RequireUserId();

            var game = await _boardService.SearchForGameAsync(userId);
            if (game == null || !game.IsGameActive) return null;

            game = _movementPieceService.SelectPieceAndHighlightMoves(game, pieceId);
            await SaveGame(game);
            return game;
        }

        public async Task<Game?> MovePiece(int x, int y)
        {
            var userId = RequireUserId();

            var game = await _boardService.SearchForGameAsync(userId);
            if (game == null || game.ActivePieceId == null)
                return null;

            var piece = game.Board.Pieces
                .FirstOrDefault(p => p.Id == game.ActivePieceId && p.Color == game.CurrentPlayerColor);

            if (piece == null)
                return game;

            var targetCell = game.Board.FindCellByCoordinates(x, y);
            var legalMoves = _rulesService.GetLegalMoves(game, piece);

            if (!legalMoves.Any(f => f.X == x && f.Y == y))
                return game;

            if (targetCell.Piece != null)
            {
                var captured = game.Board.Pieces.FirstOrDefault(p =>
                    p.Id == targetCell.Piece.Id &&
                    p.Color == targetCell.Piece.Color);

                if (captured != null)
                    captured.IsCaptured = true;
            }

            var fromCell = game.Board.FindCellByCoordinates(piece.CurrentPosition.X, piece.CurrentPosition.Y);
            if (fromCell != null)
                fromCell.Piece = null;

            piece.CurrentPosition = targetCell.Field;
            targetCell.Piece = piece;

            game.ActivePieceId = null;
            game.AvailableMoves.Clear();
            foreach (var cell in game.Board.Cells)
                cell.IsHighlighted = false;

            var currentColor = game.CurrentPlayerColor;
            var nextColor = (Color)(((int)game.CurrentPlayerColor + 1) % game.NumberOfPlayers);
            game.CurrentPlayerColor = nextColor;

            game.IsCheck = _rulesService.IsKingInCheck(game, nextColor);
            game.IsCheckmate = _rulesService.IsCheckmate(game, nextColor);
            game.IsStalemate = _rulesService.IsStalemate(game, nextColor);

            if (game.IsCheckmate || game.IsStalemate)
            {
                game.IsGameActive = false;
                if (game.IsCheckmate)
                    game.Winner = game.Players.FirstOrDefault(p => p.Colour == currentColor);
            }

            await SaveGame(game);
            return game;
        }

        public void MarkPiecesWithLegalMoves(Game game)
        {
            foreach (var cell in game.Board.Cells)
                if (cell.Piece != null)
                    cell.Piece.HasAnyLegalMove = false;

            foreach (var cell in game.Board.Cells)
            {
                var piece = cell.Piece;
                if (piece == null) continue;
                if (piece.IsCaptured || piece.Color != game.CurrentPlayerColor) continue;

                var moves = _rulesService.GetLegalMoves(game, piece);
                piece.HasAnyLegalMove = moves.Any();
            }
        }

        private ObjectId RequireUserId()
        {
            var userId = _userIdentifierService.GetUserObjectId();
            if (userId == null) throw new ArgumentException("UserId was null");
            return userId;
        }

        private Task SaveGame(Game game)
        {
            return _mongoDbService.GetGamesCollection()
                .ReplaceOneAsync(g => g.Id == game.Id, game);
        }
    }
}
