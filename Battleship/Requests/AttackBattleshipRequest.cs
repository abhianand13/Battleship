using BattleshipGame.Models.Enums;

namespace BattleshipGame.Requests
{
    public class AttackBattleshipRequest
    {
        public Player SourcePlayer { get; set; }
        public Player TargetPlayer { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
    }
}