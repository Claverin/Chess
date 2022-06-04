namespace Chess.Models
{
    public class Game
    {
        public Board Board { get; set; }
        public Color ActivePlayer { get; set; }

        public Game()
        {
            this.Board = new Board();
            this.ActivePlayer = Color.White;
        }
        
        //var playerOne = new Player()
        //playerOne.Colour = Color.White;
        //var playerTwo = new Player()
        //playerOne.Colour = Color.Black;
    }
}
