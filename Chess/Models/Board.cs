using Chess.Models.Pieces;

namespace Chess.Models
{
    public class Board
    {
        public int Id { get; set; }
        public List<Cell> Cells = new List<Cell>();
        public string? activeField { get; set; }
        public int Size { get; set; }
        public Board()
        {
            Size = 8;
            PieceIdManager.Reset();
            Create();
            PutPiecesOnBoard();
        }

        private void Create()
        {
            for (int i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    if ((i + j) % 2 == 0)
                    {
                        Cells.Add(new Cell(i, j,Color.White));
                    }
                    else
                    {
                        Cells.Add(new Cell(i, j, Color.Black));
                    }
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
            int x = color == Color.White ? 7 : 0;
            List<Cell> rulerPieces = Cells.FindAll(cell => cell.Field.x == x);

            var pieceOrder = new Piece[]
            {
                new Rock(color), new Knight(color), new Bishop(color), new Queen(color),
                new King(color), new Bishop(color), new Knight(color), new Rock(color)
            };

            for (int i = 0; i < pieceOrder.Length; i++)
            {
                rulerPieces[i].Piece = pieceOrder[i];
            }

            x = (x == 0) ? 1 : 6;
            foreach (var cell in Cells.FindAll(cell => cell.Field.x == x))
            {
                cell.Piece = new Pawn(color);
            }
        }
        public Cell FindCellByPieceId(int pieceId)
        {
            return Cells.FirstOrDefault(cell => cell.Piece != null && cell.Piece.Id == pieceId);
        }
    }
}