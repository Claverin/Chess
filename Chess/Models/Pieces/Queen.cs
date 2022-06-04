namespace Chess.Models.Pieces
{
    public class Queen : Piece
    {
        public Queen(Color color) : base(color)
        {
        }
        new public void Move(Cell fromCell, Cell toCell)
        {
            throw new NotImplementedException();
        }
        override public string GetImage()
        {
            return "/img/Queen.jpg";
        }
    }
}