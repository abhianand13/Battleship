using System.Collections.Generic;

namespace BattleshipGame.Models
{
    public class Game
    {
        private int gameBoardSize;

        public int Id { get; set; }
        public List<BoardPosition> GameBoard { get; set; }
        public bool IsInProgress { get; set; }
        public int GameBoardSize
        {
            get { return gameBoardSize; }
        }

        public Game(int gameBoardSize)
        {
            this.gameBoardSize = gameBoardSize;
            GameBoard = new List<BoardPosition>();
        }
    }
}