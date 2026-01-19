using System;
using System.Threading;
using System.Threading.Tasks;

namespace ByeByeDPI.Services
{
    public class TaskMonitor : IDisposable
    {
        public event Action<bool> OnRunningStateChanged;

        private readonly TaskService _service;
        private CancellationTokenSource _cts;

        public TaskMonitor(TaskService service)
        {
            _service = service;
        }

        public void Start(int intervalMs = 2000)
        {
            Stop();

            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        bool isRunning = _service.IsRunning;
                        OnRunningStateChanged?.Invoke(isRunning);
                    }
                    catch
                    {
                        // yut – background monitor crash etmemeli
                    }

                    await Task.Delay(intervalMs, token);
                }
            }, token);
        }

        public void Stop()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        public void Dispose()
        {
            Stop();
        }
    }
}

