using Chess.Abstractions.Services;
using Chess.Domain.Entities;
using Chess.Domain.Enums;
using Chess.Domain.ValueObjects;

namespace Chess.Services
{
    public class GameRulesService : IGameRulesService
    {
        public bool IsKingInCheck(Game game)
        {
            var king = game.Board.Pieces
                .FirstOrDefault(p => p is King && p.Color == game.CurrentPlayerColor && !p.IsCaptured);

            if (king == null)
                return false;

            var kingPos = king.CurrentPosition;

            foreach (var enemy in game.Board.Pieces.Where(p => p.Color != game.CurrentPlayerColor && !p.IsCaptured))
            {
                var possibleMoves = enemy.GetPossibleMoves(enemy.CurrentPosition, game.Board);
                if (possibleMoves.Any(f => f.X == kingPos.X && f.Y == kingPos.Y))
                    return true;
            }

            return false;
        }

        public bool IsCheckmate(Game game)
        {
            if (!IsKingInCheck(game))
                return false;

            return !game.Board.Pieces
                .Where(p => p.Color == game.CurrentPlayerColor && !p.IsCaptured)
                .Any(p => GetLegalMoves(game, p).Count > 0);
        }

        public bool IsStalemate(Game game)
        {
            if (IsKingInCheck(game))
                return false;

            return !game.Board.Pieces
                .Where(p => p.Color == game.CurrentPlayerColor && !p.IsCaptured)
                .Any(p => GetLegalMoves(game, p).Count > 0);
        }

        public List<Field> GetLegalMoves(Game game, Piece piece)
        {
            var possibleMoves = piece.GetPossibleMoves(piece.CurrentPosition, game.Board);
            var legalMoves = new List<Field>();

            var fromCell = game.Board.FindCellByCoordinates(piece.CurrentPosition.X, piece.CurrentPosition.Y);

            foreach (var move in possibleMoves)
            {
                var toCell = game.Board.FindCellByCoordinates(move.X, move.Y);
                var originalPosition = piece.CurrentPosition;
                var pieceInTarget = toCell.Piece;

                fromCell.Piece = null;
                toCell.Piece = piece;
                piece.CurrentPosition = move;

                if (!IsKingInCheck(game))
                {
                    legalMoves.Add(move);
                }

                piece.CurrentPosition = originalPosition;
                fromCell.Piece = piece;
                toCell.Piece = pieceInTarget;
            }

            return legalMoves;
        }
    }
}
