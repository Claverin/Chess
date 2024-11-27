namespace Chess.Models.Pieces
{
    public class Pawn : Piece
    {
        public Cell StartingPlace { get; set; }
        public Pawn(Color color) : base(color)
        {
            Image = "/img/" + color + "/Pawn.svg";
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