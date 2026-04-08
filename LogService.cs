using System;
using System.IO;

namespace SystemMonitorCLI.Services
{
    public class LogService
    {
        private readonly string _logFile;

        public LogService(string logFile = "system_log.txt")
        {
            _logFile = logFile;
        }

        /// <summary>
        /// 將訊息附加寫入日誌檔，包含時間戳記。
        /// </summary>
        public void WriteLog(string message)
        {
            try
            {
                string log = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {message}";
                File.AppendAllText(_logFile, log + Environment.NewLine);
            }
            catch (IOException ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"[警告] 日誌寫入失敗：{ex.Message}");
                Console.ResetColor();
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"[警告] 日誌寫入權限不足：{ex.Message}");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// 讀取所有日誌內容並回傳，若檔案不存在則回傳空字串。
        /// </summary>
        public string ReadLog()
        {
            try
            {
                return File.Exists(_logFile)
                    ? File.ReadAllText(_logFile)
                    : string.Empty;
            }
            catch (Exception ex)
            {
                return $"讀取日誌失敗：{ex.Message}";
            }
        }
    }
}

