using Chess.Abstractions.Services;
using Chess.Domain.Entities;
using Chess.Domain.Enums;

namespace Chess.Services
{
    public class BoardSetupService
    {
        public Game CreateNewBoardWithRules(int numberOfPlayers)
        {
            try
            {
                var game = new Game
                {
                    NumberOfPlayers = numberOfPlayers,
                    Board = new Board(),
                    Players = new List<Player>(),
                    DebugMode = true
                };

                if (numberOfPlayers < 2 || numberOfPlayers > 6)
                    throw new ArgumentException("Number of players must be between 2 and 6.");

                var colors = Enum.GetValues(typeof(Color)).Cast<Color>().ToList();

                for (int i = 0; i < numberOfPlayers; i++)
                {
                    var player = new Player
                    {
                        UserId = game.OwnerId,
                        Colour = colors[i]
                    };
                    game.Players.Add(player);
                }

                return game;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("SetupGameMode Error", ex);
            }
        }
    }
}