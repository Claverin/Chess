using Chess.Models;

public class Pawn : Piece
{
    public Pawn(Color color, Field position, int id) : base(color, position, id) { }

    public override List<Field> GetPossibleMoves(Field currentField, Board board)
    {

    }
}