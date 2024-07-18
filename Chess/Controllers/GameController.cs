using Chess.Data;
using Chess.Models;
using Chess.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Chess.Controllers
{
    public class GameController : Controller
    {
        private readonly ILogger<GameController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        Game game;

        public GameController(ILogger<GameController> logger, ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
        }

        public IActionResult StartGame(int numberOfPlayers)
        {
            var game = new Game(numberOfPlayers);
            HttpContext.Session.Set("Game", game);
            return RedirectToAction("GameBoard");
        }

        public IActionResult GameBoard()
        {
            var game = HttpContext.Session.Get<Game>("Game");
            if (game == null)
            {
                return RedirectToAction("StartGame");
            }
            return View(game);
        }

        public IActionResult MovePiece(int pieceId)
        {
            var game = HttpContext.Session.Get<Game>("Game");
            if (game == null)
            {
                return RedirectToAction("StartGame");
            }

            var piece = game.Board.FindPieceById(pieceId);
            if (piece != null)
            {
                
            }

            Console.WriteLine("Tst: " + pieceId);
            HttpContext.Session.Set("Game", game);
            return RedirectToAction("GameBoard");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}