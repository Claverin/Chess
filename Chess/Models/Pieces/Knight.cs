namespace Chess.Models.Pieces
{
    public class Knight : Piece
    {
        public Knight(Color color) : base(color)
        {
            Image = "/img/" + color + "/Knight.svg";
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