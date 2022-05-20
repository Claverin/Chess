namespace Chess.Models
{
    public class GameLogic
    {
        public Board[,]? Board { get; set; }
        public bool MoveValidator()
        {
            return true;
        }
        public void SetupBoard(Board board)
        {

        }
        public bool CheckMoveIsValid(Piece piece, Squere fromSquare, Squere toSquare)
        {
            return true;
        }
        public GameState GetGameState()
        {
            //InPlay CheckMate StaleMate
            GameState currentGame = new GameState();
            currentGame.State = "inPlay";

            return currentGame;
        }
    }
}
