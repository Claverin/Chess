using Chess.Models;

namespace Chess.Models
{
    public abstract class Piece
    {
        public int Id { get; set; }
        public Color Color { get; set; }
        public string Image { get; set; }
        public Field CurrentPosition { get; set; }
        public bool IsCaptured { get; set; } = false;


        protected Piece(Color color, Field currentPosition, int id)
        {
            Id = id;
            Color = color;
            Image = $"/img/{color}/{GetType().Name}.svg";
            CurrentPosition = currentPosition;
        }

        public abstract List<Field> GetPossibleMoves(Field currentPosition, Board board);
    }
}