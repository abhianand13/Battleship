using BattleshipGame.Models;
using BattleshipGame.Requests;
using BattleshipGame.Responses;

namespace BattleshipGame.Services.Interfaces
{
    public interface IBattleshipService
    {
        NewGameResponse InitializeNewGame(int gameBoardSize, bool forceCreate);
        BaseResponse AddNewBattleship(AddBattleshipRequest request);
        AttackBattleshipResponse AttackBattleship(AttackBattleshipRequest request);
        GetGameBoardResponse GetGameBoard();
    }
}