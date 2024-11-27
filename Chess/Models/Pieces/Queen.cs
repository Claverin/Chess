namespace Chess.Models.Pieces
{
    public class Queen : Piece
    {
        public Queen(Color color) : base(color)
        {
            Image = "/img/" + color + "/Queen.svg";
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