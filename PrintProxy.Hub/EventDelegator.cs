namespace PrintProxy.Hub;

public class EventDelegator
{
    public delegate void OnDelegate(object sender);

    public event OnDelegate OnEvent;

    public Task SendEventAsync(object sender)
    {
        if (OnEvent != null)
        {
            OnEvent.Invoke(sender);
        }
        return Task.CompletedTask;
    }
}

public class EventDelegator<T> where T : EventArgs
{
    public delegate void OnDelegate(object sender,T eventArgs);

    public event OnDelegate OnEvent;

    public Task SendEventAsync(object sender, T eventArgs)
    {
        if (OnEvent != null)
        {
            OnEvent.Invoke(sender,eventArgs);
        }
        return Task.CompletedTask;
    }
}