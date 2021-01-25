using BattleshipGame.Models.Enums;

namespace BattleshipGame.Requests
{
    public class AddBattleshipRequest
    {
        public Player Player { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public int ShipSize { get; set; }
        public bool IsHorizontal { get; set; }
    }
}