using Chess.Domain.Entities;
using Chess.Domain.Entities.Pieces;
using Chess.Domain.ValueObjects;

namespace Chess.Services
{
    public class GameMoveApplier
    {
        public void ApplyMove(Game game, Piece piece, Field target)
        {
            int fromX = piece.CurrentPosition.X;
            int fromY = piece.CurrentPosition.Y;

            bool isCastling = piece is King && Math.Abs(target.X - fromX) == 2;

            var fromCell = game.Board.FindCellByCoordinates(fromX, fromY);
            var toCell = game.Board.FindCellByCoordinates(target.X, target.Y);

            if (toCell.Piece != null)
                toCell.Piece.IsCaptured = true;

            fromCell.Piece = null;
            toCell.Piece = piece;
            piece.CurrentPosition = target;

            if (isCastling)
                ApplyCastlingRookMove(game, fromY, target.X);

            piece.HasMoved = true;
        }

        private void ApplyCastlingRookMove(Game game, int y, int kingToX)
        {
            int rookFromX = (kingToX == 6) ? 7 : 0;
            int rookToX = (kingToX == 6) ? 5 : 3;

            var rookCell = game.Board.FindCellByCoordinates(rookFromX, y);
            if (rookCell?.Piece is not Rook rook)
                throw new InvalidOperationException("Castling rook missing.");

            rookCell.Piece = null;

            var rookTarget = game.Board.FindCellByCoordinates(rookToX, y);
            rookTarget.Piece = rook;
            rook.CurrentPosition = rookTarget.Field;

            rook.HasMoved = true;
        }
    }
}