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

            foreach (var move in possibleMoves)
            {
                var clonedGame = game.DeepClone();
                var clonedPiece = clonedGame.Board.Pieces.First(p => p.Id == piece.Id);

                var fromCell = clonedGame.Board.FindCellByCoordinates(clonedPiece.CurrentPosition.X, clonedPiece.CurrentPosition.Y);
                var toCell = clonedGame.Board.FindCellByCoordinates(move.X, move.Y);

                fromCell.Piece = null;
                toCell.Piece = clonedPiece;
                clonedPiece.CurrentPosition = move;

                if (!IsKingInCheck(clonedGame, piece.Color))
                    legalMoves.Add(move);
            }

            return legalMoves;
        }
    }
}
