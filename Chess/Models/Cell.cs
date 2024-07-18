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
            FieldColor = color == Color.White ? "#dcd3ea" : "#8a785d";
        }

        public bool isOccupied()
        {
            return Piece != null;
        }
    }
}