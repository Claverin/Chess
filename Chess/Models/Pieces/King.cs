namespace Chess.Models.Pieces
{
    public class King : Piece
    {
        public King(Color color) : base(color)
        {
            Image = "/img/" + color + "/King.svg";
        }

        public override List<Coordinates> AvaibleMoves(Coordinates fromCell)
        {
            var movePatern = new List<Coordinates>();

            int[] dx = { 1, 1, 1, 0, 0, -1, -1, -1 };
            int[] dy = { 1, 0, -1, 1, -1, 1, 0, -1 };

            for (int i = 0; i < 8; i++)
            {
                int newX = fromCell.x + dx[i];
                int newY = fromCell.y + dy[i];

                if (newX >= 0 && newX <= 7 && newY >= 0 && newY <= 7)
                {
                    movePatern.Add(new Coordinates(newX, newY));
                }
            }
            return movePatern;
        }


        public override bool CanMove(Cell fromCell, Cell toCell)
        {
            throw new NotImplementedException();
        }
    }
}