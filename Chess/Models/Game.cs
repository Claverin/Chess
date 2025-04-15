using MongoDB.Bson;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Chess.Models
{
    public class Game
    {
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public List<Player> Players { get; set; } = new();
        public int NumberOfPlayers { get; set; }
        public Board Board { get; set; }
        public Player ActivePlayer { get; set; }
        public List<string> MoveHistory { get; set; } = new();
        public Color? Winner { get; set; } = null;
        public bool DebugMode { get; set; } = false;
        public int? ActivePieceId { get; set; } = null;
        public bool Active { get; set; } = true;
    }
}