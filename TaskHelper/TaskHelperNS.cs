using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskHelper
{
    public class TaskHelperNS : ITaskHelper
    {
        public event EventOnTaskHelperPreCallbackEvent OnTaskHelperPreCallbackEvent;
        public event EventOnTaskHelperCallbackEvent OnTaskHelperCallbackEvent;
        public event EventOnTaskHelperCancellationRequestedEvent OnTaskHelperCancellationRequestedEvent;
        public event EventOnTaskHelperCanceledExceptionEvent OnTaskHelperCanceledExceptionEvent;
        public event EventOnTaskHelperCallbackExceptionEvent OnTaskHelperCallbackExceptionEvent;

        private Task task = null;
        private CancellationTokenSource cancellationTokenSource;

        private async void TaskAction(CancellationTokenSource cancellationTokenSource)
        {
            OnTaskHelperPreCallbackEvent?.Invoke(cancellationTokenSource);

            while (true)
            {
                if (cancellationTokenSource.IsCancellationRequested)
                {
                    OnTaskHelperCancellationRequestedEvent?.Invoke();
                    break;
                }
                else
                {
                    try
                    {
                        await OnTaskHelperCallbackEvent?.Invoke(cancellationTokenSource);
                    }
                    catch (TaskCanceledException taskCanceledException)
                    {
                        OnTaskHelperCanceledExceptionEvent?.Invoke(taskCanceledException);
                        break;
                    }
                    catch (Exception exception)
                    {
                        OnTaskHelperCallbackExceptionEvent?.Invoke(exception);
                        break;
                    }
                }
            }
        }

        public bool StartTask()
        {
            if (!IsRunning)
            {
                //check if task is canceled by token/error
                if (task != null && task.Status == TaskStatus.RanToCompletion)
                {
                    Dispose();
                }

                cancellationTokenSource = new CancellationTokenSource();

                task = Task.Run(() =>
                {
                    TaskAction(cancellationTokenSource);
                }, cancellationTokenSource.Token);

                return true;
            }
            else
            {
                return false;
            }
        }
        public bool StopTask()
        {
            if (cancellationTokenSource != null &&
                !cancellationTokenSource.IsCancellationRequested)
            {
                //send cancel signal
                cancellationTokenSource.Cancel();
                //dispose objects
                Dispose();

                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsRunning
        {
            get
            {
                return task != null && task.Status != TaskStatus.RanToCompletion;
            }            
        }
        public CancellationTokenSource GetCancellationTokenSource
        {
            get
            {
                return cancellationTokenSource;
            }
        }

        private void Dispose()
        {
            //wait to task complete
            task.Wait();
            //dispose objects
            cancellationTokenSource.Dispose();
            task.Dispose();
        }
    }
}