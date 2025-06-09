using Chess.Models;

namespace Chess.Models
{
    public abstract class Piece
    {
        public int Id { get; set; }
        public Color Colour { get; set; }
        public string Image { get; set; }
        public Field CurrentPosition { get; set; }


        protected Piece(Color color, Field currentPosition, int id)
        {
            Id = id;
            Colour = color;
            Image = $"/img/{color}/{GetType().Name}.svg";
            CurrentPosition = currentPosition;
        }

        public abstract List<Field> AvailableMoves(Field CurrentPosition);
    }
}