namespace Chess.Models.Pieces
{
    public class Bishop : Piece
    {
        public Bishop(Color color) : base(color)
        {
            Image = "/img/" + color + "/Bishop.png";
        }
        new public bool Move(Cell fromCell, Cell toCell)
        {
            if(false)
            {
                return false;
            }
            return true;
        }
    }
}