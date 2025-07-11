using Chess.Models;

public class King : Piece
{
    public King(Color color, Field position, int id) : base(color, position, id) { }
    public override List<Field> GetPossibleMoves(Field current, Board board)
    {
        var moves = new List<Field>();
        int[] dx = { -1, -1, -1, 0, 1, 1, 1, 0 };
        int[] dy = { -1, 0, 1, 1, 1, 0, -1, -1 };

        for (int i = 0; i < 8; i++)
        {
            int x = current.X + dx[i];
            int y = current.Y + dy[i];
            var cell = board.FindCellByCoordinates(x, y);
            if (cell != null && (cell.Piece == null || cell.Piece.Color != Color))
                moves.Add(cell.Field);
        }

        return moves;
    }
}