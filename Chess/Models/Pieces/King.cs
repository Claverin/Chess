using Chess.Models;

public class King : Piece
{
    public King(Color color)
    {
        Colour = color;
        Image = "/img/" + color + "/King.svg";
    }
    public override List<Field> AvailableMoves(Field current)
    {
        var moves = new List<Field>();

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx != 0 || dy != 0)
                    moves.Add(new Field { x = current.x + dx, y = current.y + dy });
            }
        }

        return moves;
    }
}