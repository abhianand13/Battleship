namespace BattleshipGame.Requests
{
    public class NewGameRequest
    {
        public int GameBoardSize { get; set; }
        public bool ForceCreate { get; set; }
    }
}