using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using System.Threading;
using TaskHelper;

namespace Debug
{
    class Program
    {
        private static readonly TaskHelperNS task = new();

        static void Main(string[] args)
        {
            task.OnTaskHelperCallbackEvent += OnTaskHelperCallbackEvent;
            task.OnTaskHelperCancellationRequestedEvent += OnTaskHelperCancellationRequestedEvent;
            task.OnTaskHelperCanceledExceptionEvent += OnTaskHelperCanceledExceptionEvent;
            task.OnTaskHelperCallbackExceptionEvent += OnTaskHelperCallbackExceptionEvent;

            Start();

            //prevent to window closes
            Console.ReadKey();
        }

        private static async Task OnTaskHelperCallbackEvent(CancellationTokenSource cancellationTokenSource)
        {
            if (cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }

            #region YourAsyncCodeHer
            //print datetime
            Console.WriteLine(DateTimeOffset.Now);
            //simulate stuff code working
            await Task.Delay(1000);
            #endregion
        }
        private static void OnTaskHelperCancellationRequestedEvent()        {
            
            Console.WriteLine("task canceled by task.GetCancellationTokenSource.Cancel() before call OnTaskHelperCallbackEvent");
        }
        private static void OnTaskHelperCanceledExceptionEvent(TaskCanceledException taskCanceledException)
        {
            Console.WriteLine("task canceled by task.GetCancellationTokenSource.Cancel() inside call OnTaskHelperCallbackEvent");
        }
        private static void OnTaskHelperCallbackExceptionEvent(Exception exception)
        {
            Console.WriteLine("exception inside OnTaskHelperCallbackEvent");
        }

        public static void Start()
        {
            if (task.StartTask())
            {
                Console.WriteLine("task started");
            }
            else
            {
                Console.WriteLine("task already started");
            }
        }
        public static void Stop()
        {
            if (!task.StopTask())
            {
                Console.WriteLine("task already stopped");
            }
        }
    }
}