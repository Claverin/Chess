﻿using Chess.Models;

public class Rook : Piece
{
    public Rook(Color color, Field position, int id) : base(color, position, id) { }

    public override List<Field> GetPossibleMoves(Field current, Board board)
    {
        var moves = new List<Field>();
        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };

        for (int dir = 0; dir < 4; dir++)
        {
            int x = current.X;
            int y = current.Y;

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