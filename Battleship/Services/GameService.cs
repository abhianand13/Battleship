using BattleshipGame.Models;
using BattleshipGame.Services.Interfaces;

namespace BattleshipGame.Services
{
    public class GameService : IGameService
    {
        private Game currentGame;

        public void InitializeNewGame(Game newGame)
        {
            currentGame = newGame;
        }

        public Game GetCurrentGame()
        {
            return currentGame;
        }
    }
}