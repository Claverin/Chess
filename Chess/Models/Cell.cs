namespace Chess.Models
{
    public class Cell
    {
        public Coordinates Field { get; set; }
        public Piece? Piece { get; set; }
        public string Image { get; set; }

        public Cell(int x, int y, Color color)
        {
            Field = new Coordinates(x, y);
            Image = color == Color.White ? "white" : "black";
        }

        public bool isOccupied()
        {
            return Piece != null;
        }
    }
}