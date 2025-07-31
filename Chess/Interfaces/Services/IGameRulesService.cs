using Chess.Domain.Entities;
using Chess.Domain.Enums;
using Chess.Domain.ValueObjects;

namespace Chess.Abstractions.Services
{
    public interface IGameRulesService
    {
        bool IsKingInCheck(Game game);
        bool IsCheckmate(Game game);
        bool IsStalemate(Game game);
        List<Field> GetLegalMoves(Game game, Piece piece);
    }
}