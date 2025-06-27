using Chess.Data;
using Chess.Models;
using Chess.Models.Identity;
using Microsoft.AspNetCore.Identity;
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
        private readonly MongoDbService _mongoDbService;
        private readonly IUserIdentifierService _userIdentifierService;

        public GameController(
            ILogger<GameController> logger,
            IGameService gameService,
            MongoDbService mongoDbService,
            IUserIdentifierService userIdentifierService)
        {
            _logger = logger;
            _gameService = gameService;
            _mongoDbService = mongoDbService;
            _userIdentifierService = userIdentifierService;
        }

        [HttpGet]
        public async Task<IActionResult> StartGame(int numberOfPlayers)
        {
            try
            {
                var userId = _userIdentifierService.CreateOrGetUserObjectId();
                var game = _gameService.InitializeGame(numberOfPlayers);
                var gamesCollection = _mongoDbService.GetGamesCollection();

                await gamesCollection.InsertOneAsync(game);

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
                var userId = _userIdentifierService.CreateOrGetUserObjectId();
                var game = await _gameService.MarkPossibleMovesAsync(userId, pieceId);

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

        [HttpPost]
        public async Task<IActionResult> MovePieceTo(Field field)
        {
            var userId = _userIdentifierService.CreateOrGetUserObjectId();
            var game = await _gameService.TryMovePieceAsync(userId, field);

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