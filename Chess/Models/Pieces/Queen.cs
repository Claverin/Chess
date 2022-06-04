namespace Chess.Models.Pieces
{
    public class Queen : Piece
    {
        public Queen(Color color) : base(color)
        {
            Image = "/img/" + color + "/Queen.png";
        }
        public void Move(Cell fromCell, Cell toCell)
        {
            throw new NotImplementedException();
        }
    }
}