using BattleshipGame.Models.Enums;
using System.Text.Json.Serialization;

namespace BattleshipGame.Responses
{
    public class AttackBattleshipResponse : BaseResponse
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AttackResult Result { get; set; }
    }
}