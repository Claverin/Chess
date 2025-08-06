using Chess.Domain.ValueObjects;

namespace Chess.Domain.Entities
{
    public class Board
    {
        public List<Cell> Cells { get; set; } = new();
        public List<Piece> Pieces { get; set; } = new();

        public Board()
        {
            CreateEmptyBoard();
        }

        public void CreateEmptyBoard()
        {
            Cells.Clear();

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    var field = new Field (x,y);
                    var color = (x + y) % 2 == 0 ? "#EEEED2" : "#769656";
                    Cells.Add(new Cell
                    {
                        Field = field,
                        FieldColor = color
                    });
                }
            }
        }

        public Cell FindCellByPieceId(int pieceId)
        {
            return Cells.FirstOrDefault(c => c.Piece != null && c.Piece.Id == pieceId);
        }

        public Cell FindCellByCoordinates(int x, int y)
        {
            return Cells.FirstOrDefault(c => c.Field.X == x && c.Field.Y == y);
        }
    }
}
