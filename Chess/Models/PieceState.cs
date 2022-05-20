namespace Chess.Models
{
    public class PieceState
    {
        public Piece? Piece { get; set; }
        public bool IsActive { get; set; }
        public Squere? Square { get; set; }
    }
}
