namespace Chess.Models
{
    public class Cell
    {
        public Squere[,] Field { get; set; }
        public Piece? Piece { get; set; }
        public bool LegalMove { get; set; }
        bool IsActive { get; set; }

        public Cell(int x, int y)
        {
            Field = new Squere[x, y];
        }

        public bool isOccupied()
        {
            return Piece != null;
        }
    }
}