using Chess.Domain.Entities;
using Chess.Domain.Enums;
using Chess.Domain.ValueObjects;

namespace Chess.Abstractions.Services
{
    public interface IGameRulesService
    {
        bool IsKingInCheck(Game game, Color color);
        bool IsCheckmate(Game game, Color color);
        bool IsStalemate(Game game, Color color);
        List<Field> GetLegalMoves(Game game, Piece piece);
    }
}