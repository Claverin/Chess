namespace Chess.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public bool Winer { get; set; }
        public Color Colour { get; set; }

        public void MovePiece()
        {

        }
    }
}
