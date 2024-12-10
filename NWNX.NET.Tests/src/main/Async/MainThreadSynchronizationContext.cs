using System;
using System.Collections.Generic;
using System.Threading;

namespace NWNX.NET.Tests.Async
{
  public sealed class MainThreadSynchronizationContext : SynchronizationContext, IAwaitable
  {
    private readonly List<QueuedTask> currentWork = [];
    private readonly List<QueuedTask> queuedTasks = [];

    public IAwaiter GetAwaiter()
    {
      return new SynchronizationContextAwaiter(this);
    }

    public override void Post(SendOrPostCallback callback, object? state)
    {
      lock (queuedTasks)
      {
        queuedTasks.Add(new QueuedTask(callback, state));
      }
    }

    public override void Send(SendOrPostCallback callback, object? state)
    {
      lock (queuedTasks)
      {
        queuedTasks.Add(new QueuedTask(callback, state));
      }
    }

    public void Update()
    {
      lock (queuedTasks)
      {
        currentWork.AddRange(queuedTasks);
        queuedTasks.Clear();
      }

      try
      {
        foreach (QueuedTask task in currentWork)
        {
          task.Invoke();
        }
      }
      catch (Exception e)
      {
        Console.Error.WriteLine(e);
      }
      finally
      {
        currentWork.Clear();
      }
    }

    private readonly struct QueuedTask
    {
      private readonly SendOrPostCallback callback;
      private readonly object? state;

      public QueuedTask(SendOrPostCallback callback, object? state)
      {
        this.callback = callback;
        this.state = state;
      }

      public void Invoke()
      {
        try
        {
          callback(state);
        }
        catch (Exception e)
        {
          Console.Error.WriteLine(e);
        }
      }
    }
  }
}
