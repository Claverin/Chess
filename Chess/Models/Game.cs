using System.Text.Json.Serialization;

namespace Chess.Models
{
    public class Game
    {
        public List<Player> Players { get; set; }
        public int NumberOfPlayers { get; set; }
        public Board Board { get; set; }
        public Player ActivePlayer { get; set; }
        public Color? Winner { get; set; } = null;

        [JsonConstructorAttribute]
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
            InitPlayers(numberOfPlayers);
            CreateBoard();
            ActivePlayer = Players[0];
        }

        private void InitPlayers(int numberOfPlayers)
        {
            if (NumberOfPlayers > 6)
            {
                throw new ArgumentException("Number of players cannot exceed 6.");
            }
            NumberOfPlayers = numberOfPlayers;

            Players = new List <Player>();
            Color[] availableColors = (Color[])Enum.GetValues(typeof(Color));

            for (int i = 0; i < NumberOfPlayers; i++)
            {
                Player currentPlayer = new Player
                {
                    Colour = availableColors[i],
                    Score = 0
                };
                Players.Add(currentPlayer);
            }
        }

        private void CreateBoard()
        {
            Board = new Board();
        }
    }
}
