namespace Chess.Models
{
    public class Game
    {
        public Board board { get; set; }
        public Color activePlayer { get; set; }

        public Game()
        {
            this.board = new Board();
            this.activePlayer = Color.White;
        }

            //var playerOne = new Player()
            //playerOne.Colour = Color.White;
            //var playerTwo = new Player()
            //playerOne.Colour = Color.Black;
    }
}
