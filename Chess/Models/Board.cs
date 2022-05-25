namespace Chess.Models
{
    public class Board
    {
        readonly List<PieceState>? Pieces;
        public string? activePlayer { get; set; }
        public string? activeField { get; set; }
        public Board()
        {
            BoardCreate();
        }
        private void BoardCreate(Squere squere)
        {
        }
        public void RemovePiece(Squere squere);
        {

        }
    }
}