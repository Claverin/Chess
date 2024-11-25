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
            return View("GameBoard", game);
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
                cell.FieldColor = "#FFC300"; //yellow
                //CheckLegalMoves(pieceId);
                //WaitForUser();
                //TransferPiece();
                //RemovePiece(cell);

                HttpContext.Session.Set("Game", game);

                var testGame = HttpContext.Session.Get<Game>("Game");
            }
            else
            {
                Console.WriteLine($"Cell not found for Piece ID: {pieceId}");
            }

            return View("GameBoard", game);
        }

        public bool IsPathClear(Cell fromCell, Cell toCell)
        {
            Game game = HttpContext.Session.Get<Game>("Game");

            //up down
            if (fromCell.Field.x == toCell.Field.x)
            {
                int direction = fromCell.Field.y < toCell.Field.y ? 1 : -1;
                for (int y = fromCell.Field.y + direction; y != toCell.Field.y; y += direction)
                {
                    if (game.Board.FindCellByCoordinates(fromCell.Field.x, y).IsOccupied())
                        return false;
                }
            }
            //right left
            else if (fromCell.Field.y == toCell.Field.y)
            {
                int direction = fromCell.Field.x < toCell.Field.x ? 1 : -1;
                for (int x = fromCell.Field.x + direction; x != toCell.Field.x; x += direction)
                {
                    if (game.Board.FindCellByCoordinates(x, fromCell.Field.y).IsOccupied())
                        return false;
                }
            }
            //diagonal
            else if (Math.Abs(fromCell.Field.x - toCell.Field.x) == Math.Abs(fromCell.Field.y - toCell.Field.y))
            {
                int xDirection = fromCell.Field.x < toCell.Field.x ? 1 : -1;
                int yDirection = fromCell.Field.y < toCell.Field.y ? 1 : -1;

                int x = fromCell.Field.x + xDirection;
                int y = fromCell.Field.y + yDirection;

                while (x != toCell.Field.x && y != toCell.Field.y)
                {
                    if (game.Board.FindCellByCoordinates(x, y).IsOccupied())
                        return false;

                    x += xDirection;
                    y += yDirection;
                }
            }

            return true;
        }

        public void LockGameState(int pieceId)
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