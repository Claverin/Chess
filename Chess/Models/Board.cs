using System.Collections.Generic;
using System.Linq;

namespace Chess.Models
{
    public class Board
    {
        public List<Cell> Cells { get; set; } = new();
        public List<Piece> Pieces { get; set; } = new();

        public Board()
        {
            InitializeBoard();
            PutPiecesOnBoard();
        }

        private void InitializeBoard()
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    var field = new Field { x = x, y = y };
                    var color = (x + y) % 2 == 0 ? "#EEEED2" : "#769656";
                    Cells.Add(new Cell
                    {
                        Field = field,
                        FieldColor = color
                    });
                }
            }
        }

        private void PutPiecesOnBoard()
        {
            PutPiecesOnBoard(Color.Black);
            PutPiecesOnBoard(Color.White);
        }

        private void PutPiecesOnBoard(Color color)
        {
            int y = color == Color.White ? 7 : 0;
            List<Cell> rulerPieces = Cells.FindAll(cell => cell.Field.y == y);

            var pieceOrder = new Piece[]
            {
                new Rook(color), new Knight(color), new Bishop(color), new Queen(color),
                new King(color), new Bishop(color), new Knight(color), new Rook(color)
            };

            for (int i = 0; i < pieceOrder.Length; i++)
            {
                rulerPieces[i].Piece = pieceOrder[i];
                pieceOrder[i].CurrentPosition = rulerPieces[i].Field;
            }

            y = (y == 0) ? 1 : 6;
            foreach (var cell in Cells.FindAll(cell => cell.Field.y == y))
            {
                var pawn = new Pawn(color);
                cell.Piece = pawn;
                pawn.CurrentPosition = cell.Field;
            }
        }

        public Cell FindCellByPieceId(int pieceId)
        {
            return Cells.FirstOrDefault(c => c.Piece != null && c.Piece.Id == pieceId);
        }

        public Cell FindCellByCoordinates(int x, int y)
        {
            return Cells.FirstOrDefault(c => c.Field.x == x && c.Field.y == y);
        }
    }
}