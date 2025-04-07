using Chess.Models;

public class Knight : Piece
{
    public Knight(Color color)
    {
        Colour = color;
        Image = "/img/" + color + "/Knight.svg";
    }

    public override List<Field> AvailableMoves(Field current)
    {
        var moves = new List<Field>
        {
            new Field { x = current.x + 2, y = current.y + 1 },
            new Field { x = current.x + 2, y = current.y - 1 },
            new Field { x = current.x - 2, y = current.y + 1 },
            new Field { x = current.x - 2, y = current.y - 1 },
            new Field { x = current.x + 1, y = current.y + 2 },
            new Field { x = current.x + 1, y = current.y - 2 },
            new Field { x = current.x - 1, y = current.y + 2 },
            new Field { x = current.x - 1, y = current.y - 2 }
        };

        return moves;
    }
}