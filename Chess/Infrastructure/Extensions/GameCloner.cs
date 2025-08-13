using Chess.Domain.Entities;
using Chess.Domain.Enums;
using Chess.Domain.ValueObjects;
using MongoDB.Bson;

namespace Chess.Extensions
{
    public static class GameCloner
    {
        public static Game DeepClone(Game original)
        {
            var clonedGame = new Game
            {
                Id = original.Id,
                OwnerId = original.OwnerId,
                CurrentPlayerColor = original.CurrentPlayerColor,
                NumberOfPlayers = original.NumberOfPlayers,
                MoveHistory = new List<string>(original.MoveHistory),
                Winner = original.Winner == null ? null : new Player
                {
                    UserId = original.Winner.UserId,
                    Name = original.Winner.Name,
                    Colour = original.Winner.Colour,
                    Score = original.Winner.Score,
                    IsHuman = original.Winner.IsHuman
                },
                ActivePieceId = original.ActivePieceId,
                AvailableMoves = original.AvailableMoves.Select(f => new Field(f.X, f.Y)).ToList(),
                IsGameActive = original.IsGameActive,
                DebugMode = original.DebugMode,
                IsCheck = original.IsCheck,
                IsCheckmate = original.IsCheckmate,
                IsStalemate = original.IsStalemate
            };

            clonedGame.Players = original.Players
                .Select(p => new Player
                {
                    UserId = p.UserId,
                    Name = p.Name,
                    Colour = p.Colour,
                    Score = p.Score,
                    IsHuman = p.IsHuman
                }).ToList();

            var clonedBoard = new Board();
            clonedBoard.Cells = new List<Cell>();
            var fieldDict = new Dictionary<string, Field>();

            foreach (var cell in original.Board.Cells)
            {
                var clonedField = new Field(cell.Field.X, cell.Field.Y);
                fieldDict[$"{clonedField.X}_{clonedField.Y}"] = clonedField;

                var clonedCell = new Cell
                {
                    Field = clonedField,
                    FieldColor = cell.FieldColor,
                    IsHighlighted = cell.IsHighlighted,
                    Piece = null
                };
                clonedBoard.Cells.Add(clonedCell);
            }

            clonedBoard.Pieces = new List<Piece>();
            foreach (var piece in original.Board.Pieces)
            {
                var newPosition = fieldDict[$"{piece.CurrentPosition.X}_{piece.CurrentPosition.Y}"];
                var clonedPiece = piece.Clone(newPosition);
                clonedBoard.Pieces.Add(clonedPiece);

                var matchingCell = clonedBoard.Cells
                    .First(c => c.Field.X == newPosition.X && c.Field.Y == newPosition.Y);
                matchingCell.Piece = clonedPiece;
            }

            clonedGame.Board = clonedBoard;
            return clonedGame;
        }

        private static Piece Clone(this Piece piece, Field newPosition)
        {
            return piece switch
            {
                King => new King(piece.Color, newPosition, piece.Id) { IsCaptured = piece.IsCaptured },
                Queen => new Queen(piece.Color, newPosition, piece.Id) { IsCaptured = piece.IsCaptured },
                Rook => new Rook(piece.Color, newPosition, piece.Id) { IsCaptured = piece.IsCaptured },
                Bishop => new Bishop(piece.Color, newPosition, piece.Id) { IsCaptured = piece.IsCaptured },
                Knight => new Knight(piece.Color, newPosition, piece.Id) { IsCaptured = piece.IsCaptured },
                Pawn => new Pawn(piece.Color, newPosition, piece.Id) { IsCaptured = piece.IsCaptured },
                _ => throw new NotSupportedException($"Unknown piece type: {piece.GetType().Name}")
            };
        }
    }
}