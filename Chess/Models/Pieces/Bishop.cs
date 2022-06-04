namespace Chess.Models.Pieces
{
    public class Bishop : Piece
    {
        public Bishop(Color color) : base(color)
        {
            Image = "/img/" + color + "/Bishop.png";
        }
        new public void Move(Cell fromCell, Cell toCell)
        {
            throw new NotImplementedException();
        }
    }
}