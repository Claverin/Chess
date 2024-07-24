using Chess.Data;
using Chess.Models;
using Chess.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace Chess.Controllers
{
    public class GameController : Controller
    {
        private readonly ILogger<GameController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

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
            return View("GameBoard",game);
        }

        public IActionResult GameBoard()
        {
            Game game = HttpContext.Session.Get<Game>("Game");
            if (game == null)
            {
                return RedirectToAction("StartGame");
            }
            return View(game);
        }

        public IActionResult MovePiece(int pieceId)
        {
            Game game = HttpContext.Session.Get<Game>("Game");
            if (game == null)
            {
                return RedirectToAction("StartGame");
            }

            Cell cell = game.Board.FindCellByPieceId(pieceId);
            if (cell != null)
            {
                var previousColor = cell.FieldColor;
                cell.FieldColor = "#FFC300";
                CheckLegalMoves(pieceId);
                WaitForUser();
                TransferPiece();
                RemovePiece(cell);

                HttpContext.Session.Set("Game", game);

                var testGame = HttpContext.Session.Get<Game>("Game");
            }
            else
            {
                Console.WriteLine($"Cell not found for Piece ID: {pieceId}");
            }
            
            return View("GameBoard",game);
        }

        public void Lock()
        {
        }

        public void CheckLegalMoves(int piece)
        {
        }

        public void WaitForUser()
        {
        }

        public void TransferPiece()
        {
        }

        public void Unlock()
        {
        }
        public void RemovePiece(Cell cell)
        {
            cell.Piece = null;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}