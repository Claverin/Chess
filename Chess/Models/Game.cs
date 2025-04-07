using System.Reflection;
using System.Text.Json.Serialization;

namespace Chess.Models
{
    public class Game
    {
        public List<Player> Players { get; set; } = new();
        public int NumberOfPlayers { get; set; }
        public Board Board { get; set; }
        public Player ActivePlayer { get; set; }
        public Color? Winner { get; set; } = null;
        public bool DebugMode { get; set; } = false;
        public int? ActivePieceId { get; set; } = null;

        [JsonConstructor]
        public Game(List<Player> players, int numberOfPlayers, Board board, Player activePlayer, Color? winner)
        {
            Players = players;
            NumberOfPlayers = numberOfPlayers;
            Board = board;
            ActivePlayer = activePlayer;
            Winner = winner;
        }

        public Game(int numberOfPlayers)
        {
            NumberOfPlayers = numberOfPlayers;
            Board = new Board();
        }
    }
}