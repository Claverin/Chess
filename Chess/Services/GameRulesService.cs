using Chess.Abstractions.Services;
using Chess.Domain.Entities;
using Chess.Domain.Enums;
using Chess.Domain.ValueObjects;
using Chess.Extensions;

namespace Chess.Services
{
    public class GameRulesService : IGameRulesService
    {
        public bool IsKingInCheck(Game game, Color color)
        {
            var king = game.Board.Pieces
                .FirstOrDefault(p => p is King && p.Color == color && !p.IsCaptured);

            if (king == null)
                return false;

            var kingPos = king.CurrentPosition;

            foreach (var enemy in game.Board.Pieces.Where(p => p.Color != color && !p.IsCaptured))
            {
                var possibleMoves = enemy.GetPossibleMoves(enemy.CurrentPosition, game.Board);
                if (possibleMoves.Any(f => f.X == kingPos.X && f.Y == kingPos.Y))
                    return true;
            }

            return false;
        }

        public bool IsCheckmate(Game game, Color color)
        {
            if (!IsKingInCheck(game, color))
                return false;

            return !game.Board.Pieces
                .Where(p => p.Color == color && !p.IsCaptured)
                .Any(p => GetLegalMoves(game, p).Count > 0);
        }

        public bool IsStalemate(Game game, Color color)
        {
            if (IsKingInCheck(game, color))
                return false;

            return !game.Board.Pieces
                .Where(p => p.Color == color && !p.IsCaptured)
                .Any(p => GetLegalMoves(game, p).Count > 0);
        }

        public List<Field> GetLegalMoves(Game game, Piece piece)
        {
            var possibleMoves = piece.GetPossibleMoves(piece.CurrentPosition, game.Board);
            var legalMoves = new List<Field>();

            foreach (var move in possibleMoves)
            {
                var clonedGame = GameCloner.DeepClone(game);
                var clonedPiece = clonedGame.Board.Pieces.First(p => p.Id == piece.Id && p.Color == game.CurrentPlayerColor);

                var fromCell = clonedGame.Board.FindCellByCoordinates(clonedPiece.CurrentPosition.X, clonedPiece.CurrentPosition.Y);
                var toCell = clonedGame.Board.FindCellByCoordinates(move.X, move.Y);

                if(toCell.Piece  != null)
                {
                    toCell.Piece.IsCaptured = true;
                }

                fromCell.Piece = null;
                toCell.Piece = clonedPiece;
                clonedPiece.CurrentPosition = move;

                if (!IsKingInCheck(clonedGame, clonedPiece.Color))
                    legalMoves.Add(move);
            }

            return legalMoves;
        }
    }
}
