using Chess.Domain.Entities;
using Chess.Domain.Enums;
using Chess.Domain.ValueObjects;
using Chess.Extensions;
using Chess.Interfaces.Services;

namespace Chess.Services
{
    public class GameRulesService : IGameRulesService
    {
        public bool IsKingInCheck(Game game, Color color)
        {
            var king = game.Board.Pieces
                .FirstOrDefault(p => p is King && p.Color == color && !p.IsCaptured);

            if (king == null) return false;

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
            var possibleMoves = piece.GetPossibleMoves(piece.CurrentPosition, game.Board).ToList();

            if (piece is King king)
                AddCastlingCandidates_Light(game, king, possibleMoves);

            var legalMoves = new List<Field>();

            foreach (var move in possibleMoves)
            {
                var clonedGame = GameCloner.DeepClone(game);

                var clonedPiece = clonedGame.Board.Pieces
                    .First(p => p.Id == piece.Id && p.Color == piece.Color && !p.IsCaptured);

                int fromX = clonedPiece.CurrentPosition.X;
                int fromY = clonedPiece.CurrentPosition.Y;

                var fromCell = clonedGame.Board.FindCellByCoordinates(fromX, fromY);
                var toCell = clonedGame.Board.FindCellByCoordinates(move.X, move.Y);

                if (toCell.Piece != null)
                    toCell.Piece.IsCaptured = true;

                fromCell.Piece = null;
                toCell.Piece = clonedPiece;
                clonedPiece.CurrentPosition = move;

                if (clonedPiece is King && Math.Abs(move.X - fromX) == 2)
                {
                    ApplyCastlingRookMove(clonedGame, fromY, move.X);
                }

                if (!IsKingInCheck(clonedGame, clonedPiece.Color))
                    legalMoves.Add(move);
            }

            return legalMoves;
        }

        private void AddCastlingCandidates_Light(Game game, King king, List<Field> possibleMoves)
        {
            if (game.NumberOfPlayers != 2) return;
            if (king.HasMoved) return;
            if (king.CurrentPosition.X != 4) return;

            if (IsKingInCheck(game, king.Color)) return;

            int y = king.CurrentPosition.Y;

            if (CanCastleShort_Light(game, y))
                possibleMoves.Add(game.Board.FindCellByCoordinates(6, y).Field);

            if (CanCastleLong_Light(game, y))
                possibleMoves.Add(game.Board.FindCellByCoordinates(2, y).Field);
        }

        private bool CanCastleShort_Light(Game game, int y)
        {
            var rookCell = game.Board.FindCellByCoordinates(7, y);
            if (rookCell.Piece is not Rook rook) return false;
            if (rook.IsCaptured || rook.HasMoved) return false;

            if (game.Board.FindCellByCoordinates(5, y).Piece != null) return false;
            if (game.Board.FindCellByCoordinates(6, y).Piece != null) return false;

            return true;
        }

        private bool CanCastleLong_Light(Game game, int y)
        {
            var rookCell = game.Board.FindCellByCoordinates(0, y);
            if (rookCell.Piece is not Rook rook) return false;
            if (rook.IsCaptured || rook.HasMoved) return false;

            if (game.Board.FindCellByCoordinates(3, y).Piece != null) return false;
            if (game.Board.FindCellByCoordinates(2, y).Piece != null) return false;
            if (game.Board.FindCellByCoordinates(1, y).Piece != null) return false;

            return true;
        }

        private void ApplyCastlingRookMove(Game game, int y, int kingToX)
        {
            int rookFromX = (kingToX == 6) ? 7 : 0;
            int rookToX = (kingToX == 6) ? 5 : 3;

            var rookCell = game.Board.FindCellByCoordinates(rookFromX, y);
            if (rookCell.Piece is not Rook rook)
                return;

            rookCell.Piece = null;

            var rookTarget = game.Board.FindCellByCoordinates(rookToX, y);
            rook.CurrentPosition = rookTarget.Field;
            rookTarget.Piece = rook;
        }
    }
}
