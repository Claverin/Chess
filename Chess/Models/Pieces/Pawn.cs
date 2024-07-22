namespace Chess.Models.Pieces
{
    public class Pawn : Piece
    {
        public Cell StartingPlace { get; set; }
        public Pawn(Color color) : base(color)
        {
            Image = "/img/" + color + "/Pawn.svg";
        }
        public bool Move(Cell fromCell, Cell toCell, int BoardSize)
        {
            if(fromCell.Field.x != toCell.Field.x)
            {
                return false;
            }
            if(StartingPlace.Field.y == BoardSize)
            {
            }
            if (fromCell.Field.y-2 != toCell.Field.y) {

            }
            return true;
        }
    }
}