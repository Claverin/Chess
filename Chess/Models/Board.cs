namespace Chess.Models
{
    public class Board
    {
        public int Size { get; set; }
        public Cell[,] Grid { get; set; }
        public Board(int s)
        {
            Size = s;
            Grid = new Cell[Size, Size];
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Grid[i, j] = new Cell(i, j);
                }
            }
        }

        public void MarkNextLegalMoves( Cell currentCell, string chessPiece)
        {
            for(int i = 0; i < Size; ++i)
            {
                for( int j = 0; j < Size; ++j)
                {
                    Grid[i, j].LegalNextMove = false;
                    Grid[i,j].CurrentlyOccupied = false;
                }
            }

            switch (chessPiece)
            {
                case "King":
                    break;
                case "Queen":
                    break;
                case "Knight":
                    Grid[currentCell.RowNumber + 2, currentCell.ColumnNumber + 1].LegalNextMove = true;
                    Grid[currentCell.RowNumber + 2, currentCell.ColumnNumber - 1].LegalNextMove = true;
                    Grid[currentCell.RowNumber - 2, currentCell.ColumnNumber + 1].LegalNextMove = true;
                    Grid[currentCell.RowNumber - 2, currentCell.ColumnNumber - 1].LegalNextMove = true;
                    Grid[currentCell.RowNumber + 1, currentCell.ColumnNumber + 2].LegalNextMove = true;
                    Grid[currentCell.RowNumber + 1, currentCell.ColumnNumber - 2].LegalNextMove = true;
                    Grid[currentCell.RowNumber - 1, currentCell.ColumnNumber + 2].LegalNextMove = true;
                    Grid[currentCell.RowNumber - 1, currentCell.ColumnNumber - 2].LegalNextMove = true;
                    break;
                case "Rook":
                    break;
                case "Bishop":
                    break;
                default:
                    break;
            }
        }
    }
}