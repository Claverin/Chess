namespace Chess.Models.Pieces
{
    public class King : Piece
    {
        public King(Color color) : base(color)
        {
        }
        new public void Move(Cell fromCell, Cell toCell)
        {
            throw new NotImplementedException();
        }
        override public string GetImage()
        {
            return "/img/King.jpg";
        }
    }
}