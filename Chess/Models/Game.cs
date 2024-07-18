namespace Chess.Models
{
    public class Game
    {
        public List<Player> Players { get; set; }
        public int NumberOfPlayers { get; set; }
        public Board Board { get; set; }
        public Player ActivePlayer { get; set; }
        public Color? Winner { get; set; } = null;

        public Game(int numberOfPlayers)
        {
            InitPlayers(numberOfPlayers);
            CreateBoard();
            ActivePlayer = Players[0];
        }

        public void InitPlayers(int numberOfPlayers)
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

        public void CreateBoard()
        {
            Board = new Board();
        }
    }
}
