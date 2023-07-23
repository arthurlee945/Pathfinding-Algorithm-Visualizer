using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class UtilFunc
{
    private CancellationTokenSource cts = new CancellationTokenSource();
    private DateTime timerStarted { get; set; } = DateTime.UtcNow.AddYears(-1);
    public void Debounce(int interval, Action<object> action, object param = null)
    {
        cts.Cancel();
        cts = new CancellationTokenSource();
        Task.Run(async delegate
        {
            await Task.Delay(interval, cts.Token);
            action.Invoke(param);
        });
    }
    public void Throttle(int interval, Action<object> action, object param = null)
    {
        cts.Cancel();
        cts = new CancellationTokenSource();

        var currTime = DateTime.UtcNow;
        if (currTime.Subtract(timerStarted).TotalMilliseconds < interval)
            interval -= (int)currTime.Subtract(timerStarted).TotalMilliseconds;
        Task.Run(async delegate
        {
            await Task.Delay(interval, cts.Token);
            action.Invoke(param);
        });
    }
    ~UtilFunc()
    {
        cts.Cancel();
    }
}
