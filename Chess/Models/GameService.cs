using Chess.Data;
using Chess.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chess.Services
{
    public class GameService : IGameService
    {
        private readonly MongoDbService _mongoDbService;

        public GameService(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public Game InitializeGame(int numberOfPlayers)
        {
            var game = new Game();
            SetupGameMode(game,numberOfPlayers);
            PutPiecesOnBoard(game.Board);
            return game;
        }

        public async Task<Game?> MakeMoveAsync(string? userId, string moveNotation)
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

        private void SetupGameMode(Game game, int numberOfPlayers)
        {
            game.NumberOfPlayers = numberOfPlayers;
            game.Board = new Board();

            game.Players = new List<Player>();
            var colors = Enum.GetValues(typeof(Color)).Cast<Color>().ToList();

            for (int i = 0; i < numberOfPlayers; i++)
            {
                if (i >= colors.Count)
                    throw new ArgumentException("Number of players exceeds available colors.");

                var player = new Player
                {
                    Colour = colors[i]
                };
                game.Players.Add(player);
            }

            game.ActivePlayer = game.Players[0];
            game.DebugMode = true;
        }

        private void PutPiecesOnBoard(Board board)
        {
            PlaceMajorPieces(board, Color.White);
            PlaceMajorPieces(board, Color.Black);
        }

        private void PlaceMajorPieces(Board board, Color color)
        {
            int yBack = color == Color.White ? 7 : 0;
            int yPawn = color == Color.White ? 6 : 1;

            var pieceOrder = new Piece[]
            {
                new Rook(color), new Knight(color), new Bishop(color), new Queen(color),
                new King(color), new Bishop(color), new Knight(color), new Rook(color)
            };

            for (int x = 0; x < 8; x++)
            {
                var backCell = board.FindCellByCoordinates(x, yBack);
                backCell.Piece = pieceOrder[x];
                pieceOrder[x].CurrentPosition = backCell.Field;

                var pawn = new Pawn(color);
                var pawnCell = board.FindCellByCoordinates(x, yPawn);
                pawnCell.Piece = pawn;
                pawn.CurrentPosition = pawnCell.Field;
            }
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