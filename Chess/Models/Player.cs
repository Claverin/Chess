using MongoDB.Bson;

namespace Chess.Models
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