using System.Text.Json.Serialization;

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
            FieldColor = color == Color.White ? "#ebecd0" : "#739552";
        }

        [JsonConstructorAttribute]
        public Cell(Coordinates field, Piece? piece, string fieldColor)
        {
            Field = field;
            Piece = piece;
            FieldColor = fieldColor;
        }

        public bool IsOccupied()
        {
            return Piece != null;
        }
    }
}