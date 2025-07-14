using Chess.Domain.Enums;
using MongoDB.Bson;

namespace Chess.Domain.Entities
{
    public class Player
    {
        public ObjectId UserId { get; set; }
        public string Name { get; set; } = "Guest";
        public Color Colour { get; set; }
        public int Score { get; set; } = 0;
        public bool IsHuman { get; set; } = true;
    }
}