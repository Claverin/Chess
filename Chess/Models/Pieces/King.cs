namespace Chess.Models.Pieces
{
    public class King : Piece
    {
        public King(Color color) : base(color)
        {
            Image = "/img/" + color + "/King.svg";
        }
        public bool CanMove(Cell fromCell, Cell toCell)
        {
            throw new NotImplementedException();
        }
    }
}