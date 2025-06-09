using Chess.Models;

public class Rook : Piece
{
    public Rook(Color color, Field position, int id) : base(color, position, id) { }

    public override List<Field> AvailableMoves(Field current)
    {
        var moves = new List<Field>();

        for (int i = 1; i < 8; i++)
        {
            moves.Add(new Field { x = current.x + i, y = current.y });
            moves.Add(new Field { x = current.x - i, y = current.y });
            moves.Add(new Field { x = current.x, y = current.y + i });
            moves.Add(new Field { x = current.x, y = current.y - i });
        }

        return moves;
    }
}