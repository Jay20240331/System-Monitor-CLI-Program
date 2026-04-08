using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using SystemMonitorCLI.Models;

namespace SystemMonitorCLI.Services
{
    public class SystemMonitor : IDisposable
    {
        private PerformanceCounter? _cpuCounter;
        private PerformanceCounter? _ramCounter;
        private bool _isWindows;

        public SystemMonitor()
        {
            _isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

            if (_isWindows)
            {
                _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                _ramCounter = new PerformanceCounter("Memory", "Available MBytes");

                // 預熱：首次讀取永遠回傳 0，需等待第二次才有效
                _cpuCounter.NextValue();
                Thread.Sleep(500);
            }
        }

        public SystemStatus GetSystemStatus()
        {
            if (_isWindows && _cpuCounter != null && _ramCounter != null)
            {
                return new SystemStatus
                {
                    CpuUsage = Math.Round(_cpuCounter.NextValue(), 2),
                    AvailableMemory = _ramCounter.NextValue(),
                    Timestamp = DateTime.Now
                };
            }

            // 非 Windows 平台：回傳預設值並提示
            Console.WriteLine("[警告] PerformanceCounter 僅支援 Windows，目前回傳模擬數值。");
            return new SystemStatus
            {
                CpuUsage = 0,
                AvailableMemory = 0,
                Timestamp = DateTime.Now
            };
        }

        public void Dispose()
        {
            _cpuCounter?.Dispose();
            _ramCounter?.Dispose();
        }
    }
}

