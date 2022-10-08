using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskHelper
{
    public delegate Task EventOnTaskHelperPreCallbackEvent(CancellationTokenSource cancellationTokenSource);
    public delegate Task EventOnTaskHelperCallbackEvent(CancellationTokenSource cancellationTokenSource);
    public delegate void EventOnTaskHelperCancellationRequestedEvent();
    public delegate void EventOnTaskHelperCanceledExceptionEvent(TaskCanceledException taskCanceledException);
    public delegate void EventOnTaskHelperCallbackExceptionEvent(Exception exception);

    public interface ITaskHelper
    {
        event EventOnTaskHelperPreCallbackEvent OnTaskHelperPreCallbackEvent;
        event EventOnTaskHelperCallbackEvent OnTaskHelperCallbackEvent;
        event EventOnTaskHelperCancellationRequestedEvent OnTaskHelperCancellationRequestedEvent;
        event EventOnTaskHelperCanceledExceptionEvent OnTaskHelperCanceledExceptionEvent;
        event EventOnTaskHelperCallbackExceptionEvent OnTaskHelperCallbackExceptionEvent;
    }
}