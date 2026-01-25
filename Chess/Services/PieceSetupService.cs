using Chess.Domain.Entities;
using Chess.Domain.Entities.Pieces;
using Chess.Domain.Enums;
using Chess.Domain.ValueObjects;

namespace Chess.Services
{
    public class PieceSetupService
    {
        public Board PutPiecesOnBoard(Board board)
        {
            try
            {
                int _nextPieceId = 0;
                PlaceMajorPieces(board, Color.White, ref _nextPieceId);
                PlaceMajorPieces(board, Color.Black, ref _nextPieceId);
                return board;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("PutPiecesOnBoard Error", ex);
            }
        }

        private void PlaceMajorPieces(Board board, Color color, ref int _nextPieceId)
        {
            int figureLine = color == Color.White ? 7 : 0;
            int pawnLine = color == Color.White ? 6 : 1;

            var pieceTypes = new Type[]
            {
                typeof(Rook), typeof(Knight), typeof(Bishop), typeof(Queen),
                typeof(King), typeof(Bishop), typeof(Knight), typeof(Rook)
            };

            for (int x = 0; x < 8; x++)
            {
                var piece = CreatePiece(pieceTypes[x], color, board.FindCellByCoordinates(x, figureLine).Field, ref _nextPieceId);
                var figureCell = board.FindCellByCoordinates(x, figureLine);
                figureCell.Piece = piece;
                board.Pieces.Add(piece);

                var pawn = new Pawn(color, board.FindCellByCoordinates(x, pawnLine).Field, ++_nextPieceId);
                var pawnCell = board.FindCellByCoordinates(x, pawnLine);
                pawnCell.Piece = pawn;
                board.Pieces.Add(pawn);

            }
        }

        private Piece CreatePiece(Type pieceType, Color color, Field field, ref int nextId)
        {
            return pieceType switch
            {
                Type t when t == typeof(Rook) => new Rook(color, field, ++nextId),
                Type t when t == typeof(Knight) => new Knight(color, field, ++nextId),
                Type t when t == typeof(Bishop) => new Bishop(color, field, ++nextId),
                Type t when t == typeof(Queen) => new Queen(color, field, ++nextId),
                Type t when t == typeof(King) => new King(color, field, ++nextId),
                _ => throw new ArgumentException("Unsupported piece type"),
            };
        }
    }
}