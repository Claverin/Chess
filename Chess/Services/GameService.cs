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
        private readonly MovementPieceService _movementPieceService;
        private readonly IUserIdentifierService _userIdentifierService;
        private readonly IGameRulesService _rulesService;
        private readonly IGameRepository _gameRepository;

        public GameService(
            GameSetupService gameSetupService,
            MovementPieceService movementPieceService,
            IUserIdentifierService userIdentifierService,
            IGameRulesService rulesService,
            IGameRepository gameRepository)
        {
            _userIdentifierService = userIdentifierService;
            _gameSetupService = gameSetupService;
            _movementPieceService = movementPieceService;
            _rulesService = rulesService;
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

            game = _movementPieceService.SelectPieceAndHighlightMoves(game, pieceId);
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

            int fromX = piece.CurrentPosition.X;
            int fromY = piece.CurrentPosition.Y;
            bool isCastling = piece is King && Math.Abs(x - fromX) == 2;

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

            var fromCell = game.Board.FindCellByCoordinates(fromX, fromY);
            if (fromCell != null)
                fromCell.Piece = null;

            piece.CurrentPosition = targetCell.Field;
            targetCell.Piece = piece;

            if (isCastling)
            {
                int rookFromX = (x == 6) ? 7 : 0;
                int rookToX = (x == 6) ? 5 : 3;

                var rookCell = game.Board.FindCellByCoordinates(rookFromX, fromY);
                if (rookCell?.Piece is Rook rook)
                {
                    rookCell.Piece = null;

                    var rookTarget = game.Board.FindCellByCoordinates(rookToX, fromY);
                    rook.CurrentPosition = rookTarget.Field;
                    rookTarget.Piece = rook;

                    rook.HasMoved = true;
                }
            }

            if (piece is King k) k.HasMoved = true;
            if (piece is Rook r) r.HasMoved = true;

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
    }
}