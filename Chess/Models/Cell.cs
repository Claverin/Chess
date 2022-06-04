namespace Chess.Models
{
    public class Cell
    {
        public Coordinates Field { get; set; }
        public Piece? Piece { get; set; }
        public string FieldColor { get; set; }

        public Cell(int x, int y, Color color)
        {
            Field = new Coordinates(x, y);
            FieldColor = color == Color.White ? "#C1B7B5" : "#323232";
        }

        public bool isOccupied()
        {
            return Piece != null;
        }
    }
}