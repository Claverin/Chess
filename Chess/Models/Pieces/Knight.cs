namespace Chess.Models.Pieces
{
    public class Knight : Piece
    {
        public Knight(Color color) : base(color)
        {
            Image = "/img/" + color + "/Knight.svg";
        }

        public override List<Coordinates> AvaibleMoves(Coordinates fromCell)
        {
            var movePatern = new List<Coordinates>();

            int[] dx = { 2, 2, -2, -2, 1, 1, -1, -1 };
            int[] dy = { 1, -1, 1, -1, 2, -2, 2, -2 };

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