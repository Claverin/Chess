namespace Chess.Models
{
    public class Cell
    {
        public Field Field { get; set; }
        public string FieldColor { get; set; }
        public Piece Piece { get; set; }
        public bool AvaibleMove { get; set; } = false;

        public bool IsOccupied()
        {
            return Piece != null;
        }
    }
}