using BattleshipGame.Models.Enums;

namespace BattleshipGame.Services.Interfaces
{
    public interface ILogService
    {
        void Log(LogType logType, string message, string stackTrace = null);
    }
}