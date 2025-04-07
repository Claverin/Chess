using Chess.Models;
using System.Drawing;

namespace Chess.Services
{
    public class GameService : IGameService
    {
        public Game InitializeGame(int numberOfPlayers)
        {
            var game = new Game(numberOfPlayers);
            game.Players.Add(new Player(Models.Color.White));
            game.Players.Add(new Player(Models.Color.Black));
            game.ActivePlayer = game.Players[0];
            game.DebugMode = true;
            return game;
        }

        public Game SelectPieceAndHighlightMoves(Game game, int pieceId)
        {
            Cell selectedCell = game.Board.FindCellByPieceId(pieceId);
            if (selectedCell == null || selectedCell.Piece == null)
                return game;

            selectedCell.FieldColor = "#FFC300"; // Highlight selected cell
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
            // Up/Down
            if (fromCell.Field.x == toCell.Field.x)
            {
                int direction = fromCell.Field.y < toCell.Field.y ? 1 : -1;
                for (int y = fromCell.Field.y + direction; y != toCell.Field.y; y += direction)
                {
                    if (game.Board.FindCellByCoordinates(fromCell.Field.x, y).IsOccupied())
                        return false;
                }
            }
            // Left/Right
            else if (fromCell.Field.y == toCell.Field.y)
            {
                int direction = fromCell.Field.x < toCell.Field.x ? 1 : -1;
                for (int x = fromCell.Field.x + direction; x != toCell.Field.x; x += direction)
                {
                    if (game.Board.FindCellByCoordinates(x, fromCell.Field.y).IsOccupied())
                        return false;
                }
            }
            // Diagonal
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
    }
}