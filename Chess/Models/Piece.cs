namespace Chess.Models
{
    interface Piece
    {
        bool Color { get; set; }
        void Move(Squere fromSquare, Squere toSquare);
    }
}