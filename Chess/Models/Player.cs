namespace Chess.Models
{
    public class Player
    {
        public string Name { get; set; }
        public Color Colour { get; set; }
        public int Score { get; set; } = 0;
        public bool IsHuman { get; set; } = true;

        public Player(Color colour)
        {
            Colour = Colour;
        }

        public Player(string name, Color colour, bool isHuman = true)
        {
            Name = name;
            Colour = colour;
            IsHuman = isHuman;
        }
    }
}