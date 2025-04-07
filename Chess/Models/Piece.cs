using Chess.Models;

public abstract class Piece
{
    public int Id { get; set; }
    public Color Colour { get; set; }
    public string Image { get; set; }
    public Field CurrentPosition { get; set; }

    public abstract List<Field> AvailableMoves(Field current);
}