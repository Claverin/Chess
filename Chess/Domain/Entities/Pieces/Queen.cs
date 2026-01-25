using Chess.Domain.Entities;
using Chess.Domain.Enums;
using Chess.Domain.ValueObjects;

namespace Chess.Domain.Entities.Pieces
{
    public class Queen : Piece
    {
        public Queen(Color color, Field position, int id) : base(color, position, id) { }

        public override List<Field> GetPossibleMoves(Field currentPosition, Board board)
        {
            var rookMoves = new Rook(Color, currentPosition, Id).GetPossibleMoves(currentPosition, board);
            var bishopMoves = new Bishop(Color, currentPosition, Id).GetPossibleMoves(currentPosition, board);
            return rookMoves.Concat(bishopMoves).ToList();
        }
    }
}