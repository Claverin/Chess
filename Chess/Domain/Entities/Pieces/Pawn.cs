using Chess.Domain.Entities;
using Chess.Domain.Enums;
using Chess.Domain.ValueObjects;

public class Pawn : Piece
{
    public Pawn(Color color, Field position, int id) : base(color, position, id) { }

    public override List<Field> GetPossibleMoves(Field current, Board board)
    {
        var moves = new List<Field>();
        int direction = Color == Color.White ? -1 : 1;
        int startRow = Color == Color.White ? 6 : 1;

        int x = current.X;
        int y = current.Y;

        var oneStep = board.FindCellByCoordinates(x, y + direction);
        if (oneStep != null && oneStep.Piece == null)
        {
            moves.Add(oneStep.Field);

            var twoStep = board.FindCellByCoordinates(x, y + 2 * direction);
            if (y == startRow && twoStep?.Piece == null)
                moves.Add(twoStep.Field);
        }

        foreach (int dx in new[] { -1, 1 })
        {
            var target = board.FindCellByCoordinates(x + dx, y + direction);
            if (target?.Piece != null && target.Piece.Color != Color)
                moves.Add(target.Field);
        }

        return moves;
    }
}