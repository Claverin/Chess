using Chess.Models;

public class Bishop : Piece
{
    public Bishop(Color color)
    {
        Colour = color;
        Image = "/img/" + color + "/Bishop.svg";
    }
    public override List<Field> AvailableMoves(Field current)
    {
        var moves = new List<Field>();

        for (int i = 1; i < 8; i++)
        {
            moves.Add(new Field { x = current.x + i, y = current.y + i });
            moves.Add(new Field { x = current.x - i, y = current.y + i });
            moves.Add(new Field { x = current.x + i, y = current.y - i });
            moves.Add(new Field { x = current.x - i, y = current.y - i });
        }

        return moves;
    }
}