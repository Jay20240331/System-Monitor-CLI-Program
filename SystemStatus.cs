using System;

namespace SystemMonitorCLI.Models
{
    public class SystemStatus
    {
        public double CpuUsage { get; set; }
        public double AvailableMemory { get; set; }
        public DateTime Timestamp { get; set; }

        public override string ToString()
        {
            return $"[{Timestamp:HH:mm:ss}] CPU: {CpuUsage,6:F2}% | 可用記憶體: {AvailableMemory,8:F1} MB";
        }
    }
}
