using Chess.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Chess.Controllers
{
    public class GameController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public GameController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult GameBoard()
        {
            var game = new Game();
            game.activePlayer = Color.Black;
            var cellNumbers = game.board.Cells.Count();
            return View(cellNumbers);
        }

        public void MovePiece()
        {
            if (CheckLegalMove())
            {

            }
        }

        public bool CheckLegalMove()
        {
            return true;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}