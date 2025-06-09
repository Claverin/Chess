using Chess.Data;
using Chess.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Threading.Tasks;

namespace Chess.Services
{
    public class GameService : IGameService
    {
        private readonly MongoDbService _mongoDbService;
        private readonly BoardService _boardService;
        private readonly PieceService _pieceService;

        public GameService(MongoDbService mongoDbService, BoardService boardService, PieceService pieceService)
        {
            _mongoDbService = mongoDbService;
            _boardService = boardService;
            _pieceService = pieceService;
        }

        public Game InitializeGame(int numberOfPlayers)
        {
            var game = new Game();
            _boardService.SetupGameMode(game, numberOfPlayers);
            _pieceService.PutPiecesOnBoard(game.Board);
            return game;
        }

        public async Task<Game> MakeMove(string? userId, string moveNotation)
        {
            var gamesCollection = _mongoDbService.GetGamesCollection();
            var queryId = string.IsNullOrEmpty(userId) ? "guest" : userId;

            var game = await gamesCollection
                .Find(g => g.Active && g.Players.Any(p => p.UserId.ToString() == queryId))
                .FirstOrDefaultAsync();

            if (game == null)
                return null;

            RecordMove(game, moveNotation);

            await gamesCollection.ReplaceOneAsync(g => g.Id == game.Id, game);

            return game;
        }

        public void RecordMove(Game game, string moveNotation)
        {
            game.MoveHistory.Add(moveNotation);
            ProcessMove(game, moveNotation);
        }

        private void ProcessMove(Game game, string moveNotation)
        {
            var moveParts = moveNotation.Split(' ');
            if (moveParts.Length != 2)
                throw new ArgumentException("Invalid move notation");

            var startPosition = moveParts[0];
            var endPosition = moveParts[1];

            var startCoordinates = ConvertToCoordinates(startPosition);
            var endCoordinates = ConvertToCoordinates(endPosition);

            var startCell = game.Board.FindCellByCoordinates(startCoordinates.X, startCoordinates.Y);
            var endCell = game.Board.FindCellByCoordinates(endCoordinates.X, endCoordinates.Y);

            if (startCell?.Piece is not null && endCell is not null)
            {
                var piece = startCell.Piece;
                if (IsMoveLegal(piece, endCoordinates))
                {
                    endCell.Piece = piece;
                    startCell.Piece = null;
                    piece.CurrentPosition = endCell.Field;
                }
            }
        }

        private (int X, int Y) ConvertToCoordinates(string position)
        {
            if (string.IsNullOrWhiteSpace(position) || position.Length != 2)
                throw new ArgumentException("Invalid position format");

            char column = position[0];
            if (!char.IsLetter(column) || column < 'a' || column > 'h')
                throw new ArgumentException("Invalid column in position");

            int row = int.Parse(position[1].ToString());
            if (row < 1 || row > 8)
                throw new ArgumentException("Invalid row in position");

            int x = column - 'a';
            int y = 8 - row;

            return (x, y);
        }

        

        public Game SelectPieceAndHighlightMoves(Game game, int pieceId)
        {
            Cell selectedCell = game.Board.FindCellByPieceId(pieceId);
            if (selectedCell == null || selectedCell.Piece == null)
                return game;

            selectedCell.FieldColor = "#FFC300";
            game.ActivePieceId = pieceId;

            HighlightAvailableMoves(game, selectedCell.Piece);

            return game;
        }
        private void HighlightAvailableMoves(Game game, Piece piece)
        {
            var legalMoves = piece.AvailableMoves(piece.CurrentPosition);

            foreach (var field in legalMoves)
            {
                Cell targetCell = game.Board.FindCellByCoordinates(field.x, field.y);
                if (targetCell == null || targetCell.IsOccupied())
                    continue;

                targetCell.AvaibleMove = true;
            }
        }
        public bool IsPathClear(Game game, Cell fromCell, Cell toCell)
        {
            if (fromCell.Field.x == toCell.Field.x)
            {
                int direction = fromCell.Field.y < toCell.Field.y ? 1 : -1;
                for (int y = fromCell.Field.y + direction; y != toCell.Field.y; y += direction)
                {
                    if (game.Board.FindCellByCoordinates(fromCell.Field.x, y).IsOccupied())
                        return false;
                }
            }
            else if (fromCell.Field.y == toCell.Field.y)
            {
                int direction = fromCell.Field.x < toCell.Field.x ? 1 : -1;
                for (int x = fromCell.Field.x + direction; x != toCell.Field.x; x += direction)
                {
                    if (game.Board.FindCellByCoordinates(x, fromCell.Field.y).IsOccupied())
                        return false;
                }
            }
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
        public void ReconstructBoardFromMoves(Game game)
        {
            foreach (var moveNotation in game.MoveHistory)
            {
                ProcessMove(game, moveNotation);
            }
        }
        public bool IsMoveLegal(Piece piece, (int x, int y) target)
        {
            return piece.AvailableMoves(piece.CurrentPosition).Any(m => m.x == target.x && m.y == target.y);
        }
    }
}