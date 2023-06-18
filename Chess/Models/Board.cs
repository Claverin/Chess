using Chess.Models.Pieces;

namespace Chess.Models
{
    public class Board
    {
        public int Id { get; set; }
        public List<Cell> Cells = new List<Cell>();
        public string? activeField { get; set; }
        public int Size { get; set; } //Default
        public Board()
        {
            Size = 8;
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
            PutPiecesOnBoard(Color.White, "bottom");
        }

        private void PutPiecesOnBoard(Color color, string position = "top")
        {
            var x = 0;
            if (position == "bottom")
            {
                x = 7;
            }
            List<Cell> rulerPieces = Cells.FindAll(cell => cell.Field.x == x);
            rulerPieces[0].Piece = new Rock(color);
            rulerPieces[1].Piece = new Knight(color);
            rulerPieces[2].Piece = new Bishop(color);
            rulerPieces[3].Piece = new Queen(color);
            rulerPieces[4].Piece = new King(color);
            rulerPieces[5].Piece = new Bishop(color);
            rulerPieces[6].Piece = new Knight(color);
            rulerPieces[7].Piece = new Rock(color);
            x = x == 0 ? 1 : 6;
            foreach (var cell in Cells.FindAll(cell => cell.Field.x == x))
            {
                cell.Piece = new Pawn(color);
            }
        }
    }
}