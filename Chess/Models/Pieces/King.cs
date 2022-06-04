namespace Chess.Models.Pieces
{
    public class King : Piece
    {
        public King(Color color) : base(color)
        {
            Image = "/img/" + color + "/King.png";
        }
        new public void Move(Cell fromCell, Cell toCell)
        {
            throw new NotImplementedException();
        }
    }
}