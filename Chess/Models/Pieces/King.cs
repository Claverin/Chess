namespace Chess.Models.Pieces
{
    public class King : Piece
    {
        public King(Color color) : base(color)
        {
            Image = "/img/" + color + "/King.svg";
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