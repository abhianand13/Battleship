using BattleshipGame.Requests;
using BattleshipGame.Responses;
using BattleshipGame.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BattleshipGame.Controllers
{
    [Route("api/game")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private IBattleshipService _battleshipService;

        public GameController(IBattleshipService battleshipService)
        {
            _battleshipService = battleshipService;
        }

        /// <summary>
        /// Creates a new game board.
        /// When forceCreate is false, a new game will not be initialized if a game is currently in progress.
        /// Set forceCreate to true to discard the game in progress and create a new one.
        /// </summary>
        [HttpPost("new")]
        public NewGameResponse NewGame(NewGameRequest request)
        {
            return _battleshipService.InitializeNewGame(request.GameBoardSize, request.ForceCreate);
        }

        /// <summary>
        /// Adds a battleship to the game board.
        /// </summary>
        [HttpPost("add")]
        public BaseResponse AddBattleship(AddBattleshipRequest request)
        {
            return _battleshipService.AddNewBattleship(request);
        }

        /// <summary>
        /// Attack an opponent's battleship. Returns a response with one of "Hit", "Miss" or "Sunk" as an attack result.
        /// </summary>
        [HttpPost("attack")]
        public AttackBattleshipResponse Attack(AttackBattleshipRequest request)
        {
            return _battleshipService.AttackBattleship(request);
        }

        /// <summary>
        /// Gets the current game board showing all the battleships.
        /// </summary>
        [HttpGet("board")]
        public GetGameBoardResponse GetGameBoard()
        {
            return _battleshipService.GetGameBoard();
        }
    }
}