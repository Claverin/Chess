namespace Chess.Models
{
    public interface Piece
    {
        bool Color { get; set; }
        void Move(Squere fromSquare, Squere toSquare);
    }
}