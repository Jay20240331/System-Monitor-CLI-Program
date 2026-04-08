using System;
using System.Threading;
using System.Threading.Tasks;
using SystemMonitorCLI.Services;

class Program
{
    static void Main(string[] args)
    {
        using var monitor = new SystemMonitor();
        var alertService = new AlertService();
        var logService = new LogService();

        Console.WriteLine("=== System Monitoring CLI ===");
        Console.WriteLine("Commands: status | watch | exit");

        while (true)
        {
            Console.Write("> ");
            var input = Console.ReadLine() ?? "exit";

            switch (input.Trim().ToLower())
            {
                case "status":
                    var status = monitor.GetSystemStatus();
                    Console.WriteLine(status);
                    var alert = alertService.Check(status);
                    if (alert != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"[ALERT] {alert}");
                        Console.ResetColor();
                        logService.WriteLog(alert);
                    }
                    break;

                case "watch":
                    Console.WriteLine("監控中... （按 Enter 停止）");
                    var cts = new CancellationTokenSource();

                    var watchTask = Task.Run(() =>
                    {
                        while (!cts.Token.IsCancellationRequested)
                        {
                            var s = monitor.GetSystemStatus();
                            Console.WriteLine(s);

                            var a = alertService.Check(s);
                            if (a != null)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"[ALERT] {a}");
                                Console.ResetColor();
                                logService.WriteLog(a);
                            }

                            try
                            {
                                Task.Delay(2000, cts.Token).Wait();
                            }
                            catch (AggregateException)
                            {
                                // 取消時正常退出
                            }
                        }
                    });

                    Console.ReadLine();
                    cts.Cancel();
                    watchTask.Wait();
                    Console.WriteLine("監控已停止。");
                    break;

                case "exit":
                    Console.WriteLine("程式結束。");
                    return;

                default:
                    Console.WriteLine("未知指令，請輸入 status、watch 或 exit。");
                    break;
            }
        }
    }
}

