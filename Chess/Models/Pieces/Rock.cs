namespace Chess.Models.Pieces
{
    public class Rock : Piece
    {
        public Rock(Color color) : base(color)
        {
            Image = "/img/" + color + "/Rock.svg";
        }

        public override List<Coordinates> AvaibleMoves(Coordinates fromCell)
        {
            var movePatern = new List<Coordinates>();

            int[] dx = { 1, -1, 0, 0 };
            int[] dy = { 0, 0, 1, -1 };

            for (int i = 1; i <= 7; i++)
            {
                for (int dir = 0; dir < 4; dir++)
                {
                    int newX = fromCell.x + i * dx[dir];
                    int newY = fromCell.y + i * dy[dir];

                    if (newX >= 0 && newX <= 2 && newY >= 0 && newY <= 2)
                    {
                        movePatern.Add(new Coordinates(newX, newY));
                    }
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
