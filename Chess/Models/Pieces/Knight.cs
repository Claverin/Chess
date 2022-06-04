namespace Chess.Models.Pieces
{
    public class Knight : Piece
    {
        public Knight(Color color) : base(color)
        {
        }
        public void Move(Cell fromCell, Cell toCell)
        {
            throw new NotImplementedException();
        }
        override public string GetImage()
        {
            return "/img/Knight.jpg";
        }
    }
}