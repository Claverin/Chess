using Chess.Models;

public class Queen : Piece
{
    public Queen(Color color, Field position, int id) : base(color, position, id) { }

    public override List<Field> GetPossibleMoves(Field current, Board board)
    {
        var rookMoves = new Rook(Color, CurrentPosition, Id).GetPossibleMoves(current, board);
        var bishopMoves = new Bishop(Color, CurrentPosition, Id).GetPossibleMoves(current, board);
        return rookMoves.Concat(bishopMoves).ToList();
    }
}