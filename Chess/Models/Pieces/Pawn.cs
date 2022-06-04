namespace Chess.Models.Pieces
{
    public class Pawn : Piece
    {
        public Pawn(Color color) : base(color)
        {
            Image = "/img/" + color + "/Pawn.png";
        }
        public void Move(Cell fromCell, Cell toCell)
        {
            throw new NotImplementedException();
        }
    }
}