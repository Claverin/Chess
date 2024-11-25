namespace Chess.Models.Pieces
{
    public class Queen : Piece
    {
        public Queen(Color color) : base(color)
        {
            Image = "/img/" + color + "/Queen.svg";
        }
        public bool CanMove(Cell fromCell, Cell toCell)
        {
            throw new NotImplementedException();
        }
    }
}