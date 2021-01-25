namespace BattleshipGame.Models
{
    public class BoardPosition
    {
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public Battleship Battleship { get; set; }
        public bool IsHit { get; set; }

        public BoardPosition(int positionX, int positionY, Battleship battleship)
        {
            PositionX = positionX;
            PositionY = positionY;
            Battleship = battleship;
            IsHit = false;
        }
    }
}