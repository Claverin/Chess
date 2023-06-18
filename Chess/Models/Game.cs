namespace Chess.Models
{
    public class Game
    {
        public Board Board { get; set; }
        public Color ActivePlayer { get; set; }
        public Color Winer { get; set; }
        public bool InGame { get; set; } = false;

        public Game()
        {
            this.Board = new Board();
            var playerOne = new Player();
            var playerTwo = new Player();
            playerOne.Colour = Color.White;
            playerTwo.Colour = Color.Black;
            this.ActivePlayer = Color.White;
        }
        public bool Play()
        {
            while (InGame)
            {
                //this.ActivePlayer(MovePiece(Piece piece));
                Pick();
                CheckLegalMove();
                Place();
                //MovePiece(ActivePlayer);
                //CheckGameState();
            }
            return true;
        }
        public void MovePiece(Color correctPlayer)
        {
            if (CheckLegalMove())
            {

            }
        }
        public void Pick()
        {
            EventHandler = true;
            return true;
        }
        public void CheckLegalMove()
        {
            return true;
        }
        public bool Place()
        {
            return true;
        }
        public void CheckGameState()
        {
            return true;
        }
    }
}
