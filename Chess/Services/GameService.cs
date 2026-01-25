// GameService.cs
using Chess.Domain.Entities;
using Chess.Domain.Enums;
using Chess.Interfaces.Repository;
using Chess.Interfaces.Services;
using MongoDB.Bson;

namespace Chess.Services
{
    public class GameService : IGameService
    {
        private readonly GameSetupService _gameSetupService;
        private readonly PieceSelectionService _pieceSelectionService;
        private readonly GameMoveApplier _gameMoveApplier;
        private readonly IUserIdentifierService _userIdentifierService;
        private readonly IGameRulesService _gameRulesService;
        private readonly IGameRepository _gameRepository;

        public GameService(
            GameSetupService gameSetupService,
            PieceSelectionService pieceSelectionService,
            GameMoveApplier gameMoveApplier,
            IUserIdentifierService userIdentifierService,
            IGameRulesService gameRulesService,
            IGameRepository gameRepository)
        {
            _gameSetupService = gameSetupService;
            _pieceSelectionService = pieceSelectionService;
            _gameMoveApplier = gameMoveApplier;
            _userIdentifierService = userIdentifierService;
            _gameRulesService = gameRulesService;
            _gameRepository = gameRepository;
        }

        public async Task<Game?> GetCurrentGame()
        {
            var userId = RequireUserId();
            var active = await _gameRepository.GetActive(userId);
            return active ?? await _gameRepository.GetLast(userId);
        }

        public async Task<Game> InitializeGame(int numberOfPlayers)
        {
            var userId = RequireUserId();
            var active = await _gameRepository.GetActive(userId);

            if (active != null)
                return active;

            var game = _gameSetupService.CreateNewGame(userId, numberOfPlayers);
            await _gameRepository.Insert(game);
            return game;
        }

        public async Task<Game> CreateNewGame(int numberOfPlayers)
        {
            var userId = RequireUserId();
            await _gameRepository.DeactivateActive(userId);

            var game = _gameSetupService.CreateNewGame(userId, numberOfPlayers);
            await _gameRepository.Insert(game);
            return game;
        }

        public async Task<Game?> SelectPiece(int pieceId)
        {
            var userId = RequireUserId();

            var game = await _gameRepository.GetActive(userId);
            if (game == null || !game.IsGameActive) return null;

            game = _pieceSelectionService.SelectPieceAndHighlightMoves(game, pieceId);
            await _gameRepository.Save(game);
            return game;
        }

        public async Task<Game?> MovePiece(int x, int y)
        {
            var userId = RequireUserId();

            var game = await _gameRepository.GetActive(userId);
            if (game == null || game.ActivePieceId == null)
                return null;

            var piece = game.Board.Pieces
                .FirstOrDefault(p => p.Id == game.ActivePieceId && p.Color == game.CurrentPlayerColor);

            if (piece == null)
                return game;

            var targetCell = game.Board.FindCellByCoordinates(x, y);
            if (targetCell == null) return game;

            var legalMoves = _gameRulesService.GetLegalMoves(game, piece);

            if (!legalMoves.Any(f => f.X == x && f.Y == y))
                return game;

            _gameMoveApplier.ApplyMove(game, piece, targetCell.Field);

            game.ActivePieceId = null;
            game.AvailableMoves.Clear();
            foreach (var cell in game.Board.Cells)
                cell.IsHighlighted = false;

            var currentColor = game.CurrentPlayerColor;
            var nextColor = (Color)(((int)game.CurrentPlayerColor + 1) % game.NumberOfPlayers);
            game.CurrentPlayerColor = nextColor;

            game.IsCheck = _gameRulesService.IsKingInCheck(game, nextColor);
            game.IsCheckmate = _gameRulesService.IsCheckmate(game, nextColor);
            game.IsStalemate = _gameRulesService.IsStalemate(game, nextColor);

            if (game.IsCheckmate || game.IsStalemate)
            {
                game.IsGameActive = false;
                if (game.IsCheckmate)
                    game.Winner = game.Players.FirstOrDefault(p => p.Colour == currentColor);
            }

            await _gameRepository.Save(game);
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

                var moves = _gameRulesService.GetLegalMoves(game, piece);
                piece.HasAnyLegalMove = moves.Any();
            }
        }

        private ObjectId RequireUserId()
        {
            var userId = _userIdentifierService.GetUserObjectId();
            if (userId == null) throw new ArgumentException("UserId was null");
            return userId;
        }
    }
}
