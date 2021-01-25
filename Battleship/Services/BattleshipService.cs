using BattleshipGame.Models;
using BattleshipGame.Models.Enums;
using BattleshipGame.Requests;
using BattleshipGame.Responses;
using BattleshipGame.Services.Interfaces;
using System.Linq;

namespace BattleshipGame.Services
{
    public class BattleshipService : IBattleshipService
    {
        private ILogService logService;
        private IGameService gameService;
        private readonly int DEFAULT_GAME_BOARD_SIZE = 10;

        #region Interface implementation
        public BattleshipService(ILogService logService, IGameService gameService)
        {
            this.logService = logService;
            this.gameService = gameService;
        }

        public NewGameResponse InitializeNewGame(int gameBoardSize, bool forceCreate)
        {
            var response = new NewGameResponse { IsSuccess = false, IsGameInProgress = false, Message = string.Empty };
            if (gameBoardSize <= 0)
            {
                string message = string.Format("Invalid game board size. New game will be created with default size {0}.", DEFAULT_GAME_BOARD_SIZE);
                logService.Log(LogType.Info, message);
                response.Message += message;
                gameBoardSize = DEFAULT_GAME_BOARD_SIZE;
            }

            var currentGame = gameService.GetCurrentGame();
            if (currentGame == null || !currentGame.IsInProgress || forceCreate)
            {
                var newGame = new Game(gameBoardSize);
                gameService.InitializeNewGame(newGame);
                logService.Log(LogType.Info, "New game created.");
                response.IsSuccess = true;
                return response;
            }
            
            if (currentGame.IsInProgress)
            {
                string message = "New game could not be created. There is a game in progress.";
                logService.Log(LogType.Error, message);
                response.Message += message;
                response.IsGameInProgress = true;
            }
            return response;
        }

        public BaseResponse AddNewBattleship(AddBattleshipRequest request)
        {
            var response = new BaseResponse { IsSuccess = false };
            var currentGame = gameService.GetCurrentGame();
            
            string errorMessage = string.Empty;
            bool isValid = ValidateGameInProgress(currentGame, out errorMessage) && ValidatePlayer(request.Player, out errorMessage);
            if (!isValid)
            {
                response.Message = errorMessage;
                return response;
            }

            var originalBoard = currentGame.GameBoard.ToList();
            currentGame.IsInProgress = true;

            Battleship battleship = new Battleship(request.Player, request.ShipSize);

            int endPositionX = request.PositionX;
            int endPositionY = request.PositionY;
            if (request.IsHorizontal)
            {
                endPositionX += request.ShipSize - 1;
            }
            else
            {
                endPositionY += request.ShipSize - 1;
            }

            isValid = ValidatePositionOnBoard(currentGame, request.PositionX, request.PositionY, out errorMessage) && ValidatePositionOnBoard(currentGame, endPositionX, endPositionY, out errorMessage);
            if (!isValid)
            {
                response.Message = errorMessage;
                return response;
            }

            for (int x = request.PositionX; x <= endPositionX; x++)
            {
                for (int y = request.PositionY; y <= endPositionY; y++)
                {
                    isValid = ValidateBattleshipsOverlap(currentGame, x, y, out errorMessage);
                    if (!isValid)
                    {
                        // If an error is encountered when adding battleship to board, reset the board to previous state and exit
                        currentGame.GameBoard = originalBoard;
                        response.Message = errorMessage;
                        return response;
                    }

                    BoardPosition position = new BoardPosition(x, y, battleship);
                    currentGame.GameBoard.Add(position);
                }
            }

            logService.Log(LogType.Info, string.Format("Battleship added at ({0},{1}).", request.PositionX, request.PositionY));
            response.IsSuccess = true;
            return response;
        }

        public AttackBattleshipResponse AttackBattleship(AttackBattleshipRequest request)
        {
            var response = new AttackBattleshipResponse { IsSuccess = false, Result = AttackResult.Miss };
            var currentGame = gameService.GetCurrentGame();

            string errorMessage = string.Empty;
            bool isValid = ValidateGameInProgress(currentGame, out errorMessage) && ValidatePlayer(request.TargetPlayer, out errorMessage);
            if (!isValid)
            {
                response.Message = errorMessage;
                return response;
            }
            
            var attackPosition = currentGame.GameBoard.FirstOrDefault(x => x.Battleship.Player == request.TargetPlayer
                                                                        && x.PositionX == request.PositionX
                                                                        && x.PositionY == request.PositionY);
            if (attackPosition?.Battleship != null)
            {
                attackPosition.IsHit = true;
                response.Result = AttackResult.Hit;
                logService.Log(LogType.Info, string.Format("Battleship hit at ({0},{1}).", request.PositionX, request.PositionY));

                var isSunk = !currentGame.GameBoard.Any(x => x.Battleship.Id == attackPosition.Battleship.Id && !x.IsHit);
                if (isSunk)
                {
                    response.Result = AttackResult.Sunk;
                    logService.Log(LogType.Info, string.Format("Battleship sunk at ({0},{1}).", request.PositionX, request.PositionY));
                }
            }
            response.IsSuccess = true;
            return response;
        }

        public GetGameBoardResponse GetGameBoard()
        {
            var response = new GetGameBoardResponse { IsSuccess = false, Message = string.Empty };
            var currentGame = gameService.GetCurrentGame();
            string errorMessage = string.Empty;

            if (ValidateGameInProgress(currentGame, out errorMessage))
            {
                response.BoardPositions = currentGame.GameBoard.OrderBy(x => x.Battleship.Player).ThenBy(x => x.PositionX).ThenBy(x => x.PositionY);
                response.IsSuccess = true;
            }
            else
            {
                response.Message = errorMessage;
            }
            return response;
        }
        #endregion

        #region Private methods
        private bool ValidateGameInProgress(Game currentGame, out string errorMessage)
        {
            if (currentGame == null)
            {
                errorMessage = "No game found.";
                logService.Log(LogType.Error, errorMessage);
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }

        private bool ValidatePlayer(Player player, out string errorMessage)
        {
            if (player != Player.Player1 && player != Player.Player2)
            {
                errorMessage = string.Format("Invalid player - {0}.", player.ToString());
                logService.Log(LogType.Error, errorMessage);
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }

        private bool ValidatePositionOnBoard(Game currentGame, int x, int y, out string errorMessage)
        {
            if (x <= 0 || y <=0 || x >= currentGame.GameBoardSize || y >= currentGame.GameBoardSize)
            {
                errorMessage = string.Format("Invalid position ({0},{1}).", x, y);
                logService.Log(LogType.Error, errorMessage);
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }

        private bool ValidateBattleshipsOverlap(Game currentGame, int x, int y, out string errorMessage)
        {
            errorMessage = "Battleships cannot overlap.";
            if (currentGame.GameBoard.Any(a => a.PositionX == x && a.PositionY == y))
            {
                logService.Log(LogType.Error, errorMessage);
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }
        #endregion
    }
}