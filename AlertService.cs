using SystemMonitorCLI.Models;

namespace SystemMonitorCLI.Services
{
    public class AlertService
    {
        // 閾值提取為常數，方便日後調整或改為設定檔
        private const double CpuThreshold = 80.0;
        private const double LowMemoryThresholdMb = 500.0;

        /// <summary>
        /// 檢查系統狀態是否觸發警報。
        /// 回傳警報訊息，若無異常則回傳 null。
        /// </summary>
        public string? Check(SystemStatus status)
        {
            if (status.CpuUsage > CpuThreshold)
                return $"CPU 使用率過高：{status.CpuUsage}%";

            if (status.AvailableMemory < LowMemoryThresholdMb && status.AvailableMemory > 0)
                return $"可用記憶體不足：{status.AvailableMemory} MB";

            return null;
        }
    }
}

