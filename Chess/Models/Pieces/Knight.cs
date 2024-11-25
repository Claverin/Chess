namespace Chess.Models.Pieces
{
    public class Knight : Piece
    {
        public Knight(Color color) : base(color)
        {
            Image = "/img/" + color + "/Knight.svg";
        }
        public bool CanMove(Cell fromCell, Cell toCell)
        {
            throw new NotImplementedException();
        }
    }
}