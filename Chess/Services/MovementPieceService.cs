﻿using Chess.Abstractions.Services;
using Chess.Domain.Entities;
using Chess.Domain.ValueObjects;

namespace Chess.Services
{
    public class MovementPieceService
    {
        private readonly IGameRulesService _rulesService;

        public MovementPieceService(IGameRulesService rulesService)
        {
            _rulesService = rulesService;
        }

        public Game SelectPieceAndHighlightMoves(Game game, int pieceId)
        {
            Piece piece = game.Board.Pieces.FirstOrDefault(p => p.Id == pieceId && p.Color == game.CurrentPlayerColor);
            if (piece == null || piece.IsCaptured)
            {
                game.ActivePieceId = null;
                game.AvailableMoves.Clear();
                HighlightCells(game, new List<Field>());
                return game;
            }

            var possibleMoves = _rulesService.GetLegalMoves(game, piece);
            game.AvailableMoves = possibleMoves;
            HighlightCells(game, possibleMoves);
            game.ActivePieceId = pieceId;

            return game;
        }

        private Game HighlightCells(Game game, List<Field> possibleMoves)
        {
            foreach (var cell in game.Board.Cells)
            {
                cell.IsHighlighted = false;
            }

            foreach (var move in possibleMoves)
            {
                var cell = game.Board.FindCellByCoordinates(move.X, move.Y);
                if (cell != null)
                    cell.IsHighlighted = true;
            }
            return game;
        }
    }
}
