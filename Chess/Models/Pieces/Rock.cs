namespace Chess.Models.Pieces
{
    public class Rock : Piece
    {
        public Rock(Color color) : base(color)
        {
        }
        new public void Move(Cell fromCell, Cell toCell)
        {
            throw new NotImplementedException();
        }
        override public string GetImage()
        {
            return "/img/Rock.jpg";
        }
    }
}
