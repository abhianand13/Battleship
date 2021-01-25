using BattleshipGame.Models.Enums;
using BattleshipGame.Services.Interfaces;
using System;

namespace BattleshipGame.Services
{
    public class LogService : ILogService
    {
        public void Log(LogType logType, string message, string stackTrace = null)
        {
            Console.WriteLine(string.Format("{0}: {1}; StackTrace = {2}", logType.ToString().ToUpper(), message, stackTrace));
        }
    }
}