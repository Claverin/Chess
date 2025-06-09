using Chess.Models;

namespace Chess.Services
{
    public class PieceService
    {
        public void PutPiecesOnBoard(Board board)
        {
            try
            {
                int _nextPieceId = 0;
                PlaceMajorPieces(board, Color.White, _nextPieceId);
                PlaceMajorPieces(board, Color.Black, _nextPieceId);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("PutPiecesOnBoard Error", ex);
            }
        }

        private void PlaceMajorPieces(Board board, Color color, int _nextPieceId)
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
            switch (pieceType)
            {
                case Type t when t == typeof(Rook):
                    return new Rook(color, field, ++nextId);
                case Type t when t == typeof(Knight):
                    return new Knight(color, field, ++nextId);
                case Type t when t == typeof(Bishop):
                    return new Bishop(color, field, ++nextId);
                case Type t when t == typeof(Queen):
                    return new Queen(color, field, ++nextId);
                case Type t when t == typeof(King):
                    return new King(color, field, ++nextId);
                default:
                    throw new ArgumentException("Unsupported piece type");
            }
        }
    }
}