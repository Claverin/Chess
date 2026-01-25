using Chess.Domain.Enums;
using Chess.Domain.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace Chess.Domain.Entities
{
    public abstract class Piece
    {
        public int Id { get; set; }
        public Color Color { get; set; }
        public string Image { get; set; }
        public Field CurrentPosition { get; set; }
        public bool HasMoved { get; set; } = false;
        public bool IsCaptured { get; set; } = false;

        [BsonIgnore]
        public bool HasAnyLegalMove { get; set; } = false;


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