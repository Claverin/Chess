using Chess.Abstractions.Services;
using Chess.Domain.Entities;
using Chess.Domain.Enums;
using Chess.Infrastructure;
using Chess.Intefaces.Infrastructure;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Chess.Services
{
    public class GameService : IGameService
    {
        private readonly IMongoDbService _mongoDbService;
        private readonly GameSetupService _gameSetupService;
        private readonly BoardService _boardService;
        private readonly MovementPieceService _movementPieceService;
        private readonly IGameRulesService _rulesService;

        public GameService(IMongoDbService mongoDbService,
            GameSetupService gameSetupService,
            BoardService boardService,
            MovementPieceService movementPieceService,
            IGameRulesService rulesService)
        {
            _mongoDbService = mongoDbService;
            _gameSetupService = gameSetupService;
            _boardService = boardService;
            _movementPieceService = movementPieceService;
            _rulesService = rulesService;
        }

        public Game InitializeGame(int numberOfPlayers)
        {
            return _gameSetupService.SetupNewGame(numberOfPlayers);
        }

        public async Task<Game> MarkPossibleMoves(ObjectId userId, int pieceId)
        {
            Game game = await _boardService.SearchForGameAsync(userId);
            if (game == null || !game.IsGameActive) return null;

            game = _movementPieceService.SelectPieceAndHighlightMoves(game, pieceId);
            await _mongoDbService.GetGamesCollection().ReplaceOneAsync(g => g.Id == game.Id, game);
            return game;
        }

        public async Task<Game> TryMovePieceAsync(ObjectId userId, int x, int y)
        {
            var game = await _boardService.SearchForGameAsync(userId);
            if (game == null || game.ActivePieceId == null)
                return null;

            var piece = game.Board.Pieces
                .FirstOrDefault(p => p.Id == game.ActivePieceId && p.Color == game.CurrentPlayerColor);

            if (piece == null)
                return game;

            var targetField = game.Board.FindCellByCoordinates(x, y);
            var legalMoves = _rulesService.GetLegalMoves(game, piece);
            if (!legalMoves.Any(f => f.X == x && f.Y == y))
                return game;

            if (targetField.Piece != null)
            {
                var pieceInList = game.Board.Pieces.FirstOrDefault(p =>
                    p.Id == targetField.Piece.Id &&
                    p.Color == targetField.Piece.Color);


                if (pieceInList != null)
                {
                    pieceInList.IsCaptured = true;
                }
            }

            var fromCell = game.Board.FindCellByCoordinates(piece.CurrentPosition.X, piece.CurrentPosition.Y);
            if (fromCell != null)
                fromCell.Piece = null;

            piece.CurrentPosition = targetField.Field;
            targetField.Piece = piece;

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
                {
                    game.Winner = game.Players.FirstOrDefault(p => p.Colour == currentColor);
                }
            }

            await _mongoDbService.GetGamesCollection().ReplaceOneAsync(g => g.Id == game.Id, game);
            return game;
        }

        public void MarkPiecesWithLegalMoves(Game game)
        {
            foreach (var cell in game.Board.Cells)
            {
                if (cell.Piece != null)
                {
                    cell.Piece.HasAnyLegalMove = false;
                }
            }

            foreach (var cell in game.Board.Cells)
            {
                var piece = cell.Piece;
                if (piece == null)
                    continue;

                if (piece.IsCaptured || piece.Color != game.CurrentPlayerColor)
                    continue;

                var moves = _rulesService.GetLegalMoves(game, piece);
                piece.HasAnyLegalMove = moves.Any();
            }
        }
    }
}