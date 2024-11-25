namespace Chess.Models.Pieces
{
    public class Rock : Piece
    {
        public Rock(Color color) : base(color)
        {
            Image = "/img/" + color + "/Rock.svg";
        }
        public bool CanMove(Cell fromCell, Cell toCell)
        {
            bool isVerticalMove = fromCell.Field.x == toCell.Field.x;
            bool isHorizontalMove = fromCell.Field.y == toCell.Field.y;

            if (!isVerticalMove && !isHorizontalMove)
                return false;

            //if (IsPathClear(fromCell, toCell))
                //return false;

            return true;
        }
    }
}
