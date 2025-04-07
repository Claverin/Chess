using Chess.Data;
using Chess.Models;
using Chess.Services;
using Chess.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Chess.Controllers
{
    public class GameController : Controller
    {
        private readonly ILogger<GameController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IGameService _gameService;

        public GameController(
            ILogger<GameController> logger,
            ApplicationDbContext db,
            UserManager<IdentityUser> userManager,
            IGameService gameService)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
            _gameService = gameService;
        }

        public IActionResult StartGame(int numberOfPlayers)
        {
            var game = _gameService.InitializeGame(numberOfPlayers);
            HttpContext.Session.Set("Game", game);
            return View("GameBoard", game);
        }

        public IActionResult MovePiece(int pieceId)
        {
            var game = HttpContext.Session.Get<Game>("Game");
            if (game == null)
                return RedirectToAction("StartGame");

            var updatedGame = _gameService.SelectPieceAndHighlightMoves(game, pieceId);
            HttpContext.Session.Set("Game", updatedGame);
            return View("GameBoard", updatedGame);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
