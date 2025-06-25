using Chess.Models;

public class King : Piece
{
    public King(Color color, Field position, int id) : base(color, position, id) { }
    public override List<Field> GetPossibleMoves(Field current, Board board)
    {
        var moves = new List<Field>();

        return moves;
    }
}