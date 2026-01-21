using Chess.Domain.Entities;
using Chess.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Chess.Controllers
{
    public class GameController : Controller
    {
        private readonly ILogger<GameController> _logger;
        private readonly IGameService _gameService;

        public GameController(ILogger<GameController> logger, IGameService gameService)
        {
            _logger = logger;
            _gameService = gameService;
        }

        [HttpGet]
        public async Task<IActionResult> Board()
        {
            try
            {
                var game = await _gameService.GetCurrentGame();
                if (game == null)
                    return View("NoActiveGame");

                _gameService.MarkPiecesWithLegalMoves(game);
                return View("GameBoard", game);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Board error");
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InitializeGame(int numberOfPlayers = 2)
        {
            try
            {
                await _gameService.InitializeGame(numberOfPlayers);
                return RedirectToAction(nameof(Board));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "StartGame error");
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNewGame(int numberOfPlayers = 2)
        {
            try
            {
                await _gameService.CreateNewGame(numberOfPlayers);
                return RedirectToAction(nameof(Board));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "NewGame error");
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SelectPiece(int pieceId)
        {
            try
            {
                await _gameService.SelectPiece(pieceId);
                return RedirectToAction(nameof(Board));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SelectPiece error");
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MovePieceTo(int x, int y)
        {
            try
            {
                var game = await _gameService.MovePiece(x, y);
                _logger.LogInformation("After move: game null={IsNull}, active={Active}, check={Check}, mate={Mate}, stalemate={Stalemate}",
                    game == null, game?.IsGameActive, game?.IsCheck, game?.IsCheckmate, game?.IsStalemate);

                return RedirectToAction(nameof(Board));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MovePieceTo error");
                return View("Error");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
            => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
