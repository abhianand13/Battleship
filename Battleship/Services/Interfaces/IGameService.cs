using BattleshipGame.Models;

namespace BattleshipGame.Services.Interfaces
{
    public interface IGameService
    {
        void InitializeNewGame(Game newGame);
        Game GetCurrentGame();
    }
}