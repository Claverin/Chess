using Chess.Models;

public class Pawn : Piece
{
    public Pawn(Color color)
    {
        Colour = color;
        Image = "/img/" + color + "/Pawn.svg";
    }

    public override List<Field> AvailableMoves(Field current)
    {
        var moves = new List<Field>();
        int direction = (Colour == Color.White) ? -1 : 1;

        // Ruch do przodu
        moves.Add(new Field { x = current.x, y = current.y + direction });

        // Początkowy podwójny ruch
        if ((Colour == Color.White && current.y == 6) || (Colour == Color.Black && current.y == 1))
        {
            moves.Add(new Field { x = current.x, y = current.y + 2 * direction });
        }

        // Bicie (tylko do sprawdzenia później, czy można)
        moves.Add(new Field { x = current.x - 1, y = current.y + direction });
        moves.Add(new Field { x = current.x + 1, y = current.y + direction });

        return moves;
    }
}