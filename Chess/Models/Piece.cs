using Chess.Utility;
using System.Text.Json.Serialization;

namespace Chess.Models
{
    [JsonConverter(typeof(PieceConverter))]
    public abstract class Piece 
    {
        private static int _nextId = 0;
        public int Id { get; set; }
        public string Image { get; set; }
        public Color Color { get; set; }
        public bool Active { get; set; }
        public Coordinates CurrentPosition { get; set; }

        public Piece(Color color)
        {
            Id = PieceIdManager.GetNextId();
            Color = color;
            Active = true;
        }

        public abstract void AvaibleMoves(Coordinates fromCell);
        public abstract bool CanMove(Cell fromCell, Cell toCell);

        public void Capture()
        {
            Active = false;
        }
    }
}