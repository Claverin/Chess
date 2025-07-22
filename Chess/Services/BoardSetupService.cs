using Chess.Abstractions.Services;
using Chess.Domain.Entities;
using Chess.Domain.Enums;

namespace Chess.Services
{
    public class BoardSetupService
    {
        private readonly IUserIdentifierService _userIdentifierService;

        public BoardSetupService(IUserIdentifierService userIdentifierService)
        {
            _userIdentifierService = userIdentifierService;
        }

        public Game CreateNewBoardWithRules(int numberOfPlayers)
        {
            try
            {
                var game = new Game();

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
                        UserId = _userIdentifierService.GetUserObjectId(),
                        Colour = colors[i]
                    };
                    game.Players.Add(player);
                }

                game.DebugMode = true;


                return game;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("SetupGameMode Error", ex);
            }
        }
    }
}