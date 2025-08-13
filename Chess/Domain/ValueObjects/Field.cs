using System.Transactions;

namespace Chess.Domain.ValueObjects
{
    public class Field
    {
        public Field(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }
    }
}