using BattleshipGame.Models;
using System.Collections.Generic;

namespace BattleshipGame.Responses
{
    public class GetGameBoardResponse : BaseResponse
    {
        public IEnumerable<BoardPosition> BoardPositions { get; set; }
    }
}
