namespace Chess.Models
{
    public class Cell
    {
        public Coordinates Field { get; set; }
        public Piece? Piece { get; set; }

        public Cell(int x, int y)
        {
            Field = new Coordinates(x, y);
        }

        public bool isOccupied()
        {
            return Piece != null;
        }
    }
}