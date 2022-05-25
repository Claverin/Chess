namespace Chess.Models
{
    public class Cell
    {
        public Squere[,] Field { get; set; }
        public Piece? Piece { get; set; }
        public bool LegalMove { get; set; }
        bool IsActive { get; set; }

        public Cell(int x, int y, Piece? piece = null, bool legalMove = false)
        {
            Field = [x, y];
            Piece = piece;
            LegalMove = legalMove;
        }

        public bool isOccupied()
        {
            return Piece != null;
        }
    }
}