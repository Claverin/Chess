using MongoDB.Bson;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Chess.Models
{
    public class Game
    {
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public Board Board { get; set; }
        public List<Player> Players { get; set; } = new();
        public Color PlayerOnMove { get; set; } = Color.White;
        public int NumberOfPlayers { get; set; }
        public List<string> MoveHistory { get; set; } = new();
        public Player? Winner { get; set; } = null;
        public int? ActivePieceId { get; set; } = null;
        public List<Field> AvailableMoves { get; set; } = new();
        public bool IsGameActive { get; set; } = true;
        public bool DebugMode { get; set; } = false;
    }
}