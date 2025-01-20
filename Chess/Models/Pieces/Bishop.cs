namespace Chess.Models.Pieces
{
    public class Bishop : Piece
    {
        public Bishop(Color color) : base(color)
        {
            Image = "/img/" + color + "/Bishop.svg";
        }

        public override List<Coordinates> AvaibleMoves(Coordinates fromCell)
        {
            var movePatern = new List<Coordinates>();

            int[] dx = { 1, 1, -1, -1 };
            int[] dy = { 1, -1, 1, -1 };

            for (int i = 1; i <= 7; i++)
            {
                for (int dir = 0; dir < 4; dir++)
                {
                    int newX = fromCell.x + i * dx[dir];
                    int newY = fromCell.y + i * dy[dir];

                    if (newX >= 0 && newX <= 7 && newY >= 0 && newY <= 7)
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