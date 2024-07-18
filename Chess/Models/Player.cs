namespace Chess.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public Color Colour { get; set; }
        public int Score { get; set; }
    }
}