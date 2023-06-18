namespace Chess.Models
{
    public abstract class Piece 
    {
        public string Image { get; set; }
        public Color Color { get; set; }
        public bool IsActive { get; set; }

        public Piece(Color color)
        {
            this.Color = color;
            this.IsActive = false;
        }
    }
}