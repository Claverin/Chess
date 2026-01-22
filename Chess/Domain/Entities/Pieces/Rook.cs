using Chess.Domain.Entities;
using Chess.Domain.Enums;
using Chess.Domain.ValueObjects;

public class Rook : Piece
{
    public bool HasMoved { get; set; } = false;

    public Rook(Color color, Field position, int id) : base(color, position, id) { }

    public override List<Field> GetPossibleMoves(Field currentPosition, Board board)
    {
        var moves = new List<Field>();
        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };

        for (int dir = 0; dir < 4; dir++)
        {
            int x = currentPosition.X;
            int y = currentPosition.Y;

            while (true)
            {
                x += dx[dir];
                y += dy[dir];

                var cell = board.FindCellByCoordinates(x, y);
                if (cell == null) break;

                if (cell.Piece == null)
                    moves.Add(cell.Field);
                else
                {
                    if (cell.Piece.Color != Color)
                        moves.Add(cell.Field);
                    break;
                }
            }
        }

        return moves;
    }
}