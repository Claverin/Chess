namespace Chess.Models.Pieces
{
    public class Knight : Piece
    {
        public Knight(Color color) : base(color)
        {
            Image = "/img/" + color + "/Knight.png";
        }
        public void Move(Cell fromCell, Cell toCell)
        {
            throw new NotImplementedException();
        }
    }
}