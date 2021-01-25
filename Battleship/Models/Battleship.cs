using BattleshipGame.Models.Enums;
using System;

namespace BattleshipGame.Models
{
    public class Battleship
    {
        public Guid Id { get; set; }
        public int Size { get; set; }
        public Player Player { get; set; }

        public Battleship(Player player, int size)
        {
            Id = Guid.NewGuid();
            Player = player;
            Size = size;
        }
    }
}