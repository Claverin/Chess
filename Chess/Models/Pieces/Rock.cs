namespace Chess.Models.Pieces
{
    public class Rock : Piece
    {
        public Rock(Color color) : base(color)
        {
            Image = "/img/" + color + "/Rock.png";
        }
        public void Move(Cell fromCell, Cell toCell)
        {
            throw new NotImplementedException();
        }
    }
}
