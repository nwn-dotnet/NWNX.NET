using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace NWNX.NET.Tests.Async
{
  public static class NwTask
  {
    public static MainThreadSynchronizationContext MainThreadSynchronizationContext { private get; set; } = null!;

    /// <summary>
    /// Waits until the specified amount of time has passed.
    /// </summary>
    /// <param name="delay">How long to wait.</param>
    /// <param name="cancellationToken">A cancellation token that should be used to cancel the work.</param>
    public static Task Delay(TimeSpan delay, CancellationToken? cancellationToken = null)
    {
      Stopwatch stopwatch = Stopwatch.StartNew();
      return RunAndAwait(() => delay < stopwatch.Elapsed, cancellationToken);
    }

    /// <summary>
    /// Waits until the specified amount of frames have passed.
    /// </summary>
    /// <param name="frames">The number of frames to wait.</param>
    /// <param name="cancellationToken">A cancellation token that should be used to cancel the work.</param>
    public static Task DelayFrame(int frames, CancellationToken? cancellationToken = null)
    {
      return RunAndAwait(() =>
      {
        bool retVal = frames <= 0;
        frames--;
        return retVal;
      }, cancellationToken);
    }

    /// <summary>
    /// Waits until the next server frame/loop.
    /// </summary>
    public static Task NextFrame()
    {
      return DelayFrame(1);
    }

    /// <summary>
    /// Queues the specified work to run on the next server cycle.
    /// </summary>
    /// <param name="function">The task to run.</param>
    public static async Task Run(Func<Task> function)
    {
      await SwitchToMainThread();
      await function();
    }

    /// <inheritdoc cref="Run"/>
    public static async Task<T> Run<T>(Func<Task<T>> function)
    {
      await SwitchToMainThread();
      return await function();
    }

    /// <summary>
    /// Safely returns to a NWScript context from another thread.<br/>
    /// This must ALWAYS be called after an external callback, or thread switch before using any NWN APIs.
    /// </summary>
    [Pure]
    public static IAwaitable SwitchToMainThread()
    {
      return MainThreadSynchronizationContext;
    }

    /// <summary>
    /// Waits until the specified expression returns true.
    /// </summary>
    /// <param name="test">The test expression.</param>
    /// <param name="cancellationToken">A cancellation token that should be used to cancel the work.</param>
    public static Task WaitUntil(Func<bool> test, CancellationToken? cancellationToken = null)
    {
      return RunAndAwait(test, cancellationToken);
    }

    /// <summary>
    /// Waits until the specified value source changes.
    /// </summary>
    /// <param name="valueSource">The watched value source.</param>
    /// <param name="cancellationToken">A cancellation token that should be used to cancel the work.</param>
    public static Task WaitUntilValueChanged<T>(Func<T> valueSource, CancellationToken? cancellationToken = null)
    {
      T currentVal = valueSource();
      return RunAndAwait(() => !Equals(currentVal, valueSource()), cancellationToken);
    }

    /// <summary>
    /// Waits until all the specified tasks have completed.
    /// </summary>
    /// <param name="tasks">The tasks to wait on for completion.</param>
    public static async Task WhenAll(params Task[] tasks)
    {
      await Task.WhenAll(tasks);
      await SwitchToMainThread();
    }

    /// <inheritdoc cref="WhenAll(System.Threading.Tasks.Task[])"/>
    public static async Task WhenAll(IEnumerable<Task> tasks)
    {
      await Task.WhenAll(tasks);
      await SwitchToMainThread();
    }

    /// <inheritdoc cref="WhenAll(System.Threading.Tasks.Task[])"/>
    public static async Task<TResult[]> WhenAll<TResult>(params Task<TResult>[] tasks)
    {
      TResult[] results = await Task.WhenAll(tasks);
      await SwitchToMainThread();
      return results;
    }

    /// <inheritdoc cref="WhenAll(System.Threading.Tasks.Task[])"/>
    public static async Task<TResult[]> WhenAll<TResult>(IEnumerable<Task<TResult>> tasks)
    {
      TResult[] results = await Task.WhenAll(tasks);
      await SwitchToMainThread();
      return results;
    }

    /// <summary>
    /// Waits until any of the specified tasks have completed.<br/>
    /// NOTE: This will not cancel other tasks that have not finished running. Specify a common <see cref="CancellationToken"/> in each of the source tasks.
    /// </summary>
    /// <param name="tasks">The tasks to wait on for completion.</param>
    public static async Task WhenAny(params Task[] tasks)
    {
      await Task.WhenAny(tasks);
      await SwitchToMainThread();
    }

    /// <inheritdoc cref="WhenAny(System.Threading.Tasks.Task[])"/>
    public static async Task WhenAny(IEnumerable<Task> tasks)
    {
      await Task.WhenAny(tasks);
      await SwitchToMainThread();
    }

    /// <inheritdoc cref="WhenAny(System.Threading.Tasks.Task[])"/>
    public static async Task<Task<TResult>> WhenAny<TResult>(params Task<TResult>[] tasks)
    {
      Task<TResult> results = await Task.WhenAny(tasks);
      await SwitchToMainThread();
      return results;
    }

    /// <inheritdoc cref="WhenAny(System.Threading.Tasks.Task[])"/>
    public static async Task<Task<TResult>> WhenAny<TResult>(IEnumerable<Task<TResult>> tasks)
    {
      Task<TResult> results = await Task.WhenAny(tasks);
      await SwitchToMainThread();
      return results;
    }

    private static async Task RunAndAwait(Func<bool> completionSource, CancellationToken? cancellationToken)
    {
      try
      {
        do
        {
          await MainThreadSynchronizationContext;
        }
        while (!completionSource() && cancellationToken is not { IsCancellationRequested: true });
      }
      catch (Exception e)
      {
        Console.Error.WriteLine(e);
      }
    }
  }
}
