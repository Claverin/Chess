namespace Chess.Models
{
    public class Board
    {
        public List<Cell> Cells;
        public string? activePlayer { get; set; }
        public string? activeField { get; set; }
        public Board()
        {
            Create();
            PutPieces();
        }

        private void Create()
        {
            var board = new Board();

            for (int i = 0; i < 8; i++)
            {
                for(var j = 0; j < 8; j++)
                {
                    var cellss = new Cell(i, j);
                    Cells.Add(cellss);
                }
            }
        }
        private void PutPieces()
        {
            foreach(var cell in Cells)
            {
                //Piony
                if(cell.Field == [,2] || [,7])
                {
                    cell.Piece = Piece.Pawn;
                    cell.Piece.Color = false;
                }
                //Wieże
                if (cell.Field == [1, 1] || [1, 8] || [8, 1] || [8, 8])
                {
                    cell.Piece = Rock;
                }
                //Konie
                if (cell.Field == [1, 2] || [1, 7] || [8, 2] || [8, 7])
                {
                    cell.Piece = Knight;
                }
                //Bishop
                if (cell.Field == [1, 3] || [1, 6] || [8, 3] || [8, 6])
                {
                    cell.Piece = Bishop;
                }
                //Królowa
                if (cell.Field == [1, 5] || [8, 6])
                {
                    cell.Piece = Queen;
                }
                //Król
                if (cell.Field == [1, 6] || [8, 5])
                {
                    cell.Piece = King;
                }
            }
        }
    }
}