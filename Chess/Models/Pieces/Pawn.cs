namespace Chess.Models.Pieces
{
    public class Pawn : Piece
    {
        public Cell StartingPlace { get; set; }
        public Pawn(Color color) : base(color)
        {
            Image = "/img/" + color + "/Pawn.svg";
        }

        public override List<Coordinates> AvaibleMoves(Coordinates fromCell)
        {
            var movePatern = new List<Coordinates> { };

            movePatern.Add(new Coordinates(fromCell.x, fromCell.y - 1));
            movePatern.Add(new Coordinates(fromCell.x, fromCell.y - 2));

            return movePatern;
        }

        public override bool CanMove(Cell fromCell, Cell toCell)
        {
            throw new NotImplementedException();
        }
    }
}