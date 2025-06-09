using Chess.Models;

namespace Chess.Services
{
    public class BoardService
    {
        public void SetupGameMode(Game game, int numberOfPlayers)
        {
            try
            {
                if (numberOfPlayers < 2 || numberOfPlayers > 6)
                    throw new ArgumentException("Number of players must be between 2 and 6.");

                game.NumberOfPlayers = numberOfPlayers;
                game.Board = new Board();
                game.Players = new List<Player>();

                var colors = Enum.GetValues(typeof(Color)).Cast<Color>().ToList();

                for (int i = 0; i < numberOfPlayers; i++)
                {
                    var player = new Player
                    {
                        Colour = colors[i]
                    };
                    game.Players.Add(player);
                }

                game.DebugMode = true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("SetupGameMode Error", ex);
            }
        }
    }
}