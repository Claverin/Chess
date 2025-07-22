using Chess.Abstractions.Services;
using Chess.Domain.Entities;
using Chess.Intefaces.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics;

namespace Chess.Controllers
{
    public class GameController : Controller
    {
        private readonly ILogger<GameController> _logger;
        private readonly IGameService _gameService;
        private readonly IMongoDbService _mongoDbService;
        private readonly IUserIdentifierService _userIdentifierService;
        private readonly IGameTrackerService _gameTrackerService;

        public GameController(
            ILogger<GameController> logger,
            IGameService gameService,
            IMongoDbService mongoDbService,
            IUserIdentifierService userIdentifierService,
            IGameTrackerService gameTrackerService)
        {
            _logger = logger;
            _gameService = gameService;
            _mongoDbService = mongoDbService;
            _userIdentifierService = userIdentifierService;
            _gameTrackerService = gameTrackerService;
        }

        [HttpGet]
        public async Task<IActionResult> StartGame(int numberOfPlayers)
        {
            try
            {
                var userId = _userIdentifierService.GetUserObjectId();
                var game = _gameService.InitializeGame(numberOfPlayers);
                await _mongoDbService.GetGamesCollection().InsertOneAsync(game);

                _gameTrackerService.SetCurrentGameId(game.Id);

                return RedirectToAction("LoadGame", new { id = game.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "StartGame error");
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> SelectPiece(int pieceId)
        {
            try
            {
                var userId = _userIdentifierService.GetUserObjectId();
                var game = await _gameService.MarkPossibleMoves(userId, pieceId);

                if (game == null)
                    return RedirectToAction("StartGame");

                return RedirectToAction("LoadGame", new { id = game.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SelectPiece action error");
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> MovePieceTo(int x, int y)
        {
            var userId = _userIdentifierService.GetUserObjectId();
            var game = await _gameService.TryMovePieceAsync(userId, x, y);

            if (game == null)
                return RedirectToAction("StartGame");

            return RedirectToAction("LoadGame", new { id = game.Id });
        }

        [HttpGet]
        public async Task<IActionResult> LoadGame(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return RedirectToAction("StartGame");

            var game = await _mongoDbService.GetGamesCollection()
                .Find(g => g.Id == objectId)
                .FirstOrDefaultAsync();

            if (game == null)
                return RedirectToAction("StartGame");

            return View("GameBoard", game);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}