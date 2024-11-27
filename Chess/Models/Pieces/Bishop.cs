namespace Chess.Models.Pieces
{
    public class Bishop : Piece
    {
        public Bishop(Color color) : base(color)
        {
            Image = "/img/" + color + "/Bishop.svg";
        }

        public override void AvaibleMoves(Coordinates fromCell)
        {
            throw new NotImplementedException();
        }

        public override bool CanMove(Cell fromCell, Cell toCell)
        {
            throw new NotImplementedException();
        }
    }
}