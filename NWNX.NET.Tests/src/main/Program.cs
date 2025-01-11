using System;
using System.Runtime.InteropServices;
using NWNX.NET.Native;
using NWNX.NET.Tests.Async;

namespace NWNX.NET.Tests
{
  public static unsafe class Program
  {
    private static readonly MainThreadSynchronizationContext TestSyncContext = new MainThreadSynchronizationContext();
    private static readonly TestRunner TestRunner = new TestRunner(TestSyncContext);

    public static event Action<ulong> OnMainLoop;

    public static void Main()
    {
      NwTask.MainThreadSynchronizationContext = TestSyncContext;

      PrelinkLibraryImports();
      NWNXAPI.RegisterSignalHandler(&NWNXSignalHandler);
      NWNXAPI.RegisterMainLoopHandler(&MainLoopHandler);
    }

    [UnmanagedCallersOnly]
    private static void NWNXSignalHandler(IntPtr signalPtr)
    {
      string? signal = signalPtr.ReadNullTerminatedString();
      if (string.IsNullOrEmpty(signal))
      {
        throw new Exception("Received null signal from NWNX.");
      }

      Console.WriteLine($"Signal received: '{signal}'");
      switch (signal)
      {
        case "ON_NWNX_LOADED":
          break;
        case "ON_MODULE_LOAD_FINISH":
          Console.WriteLine("Running tests.");
          TestRunner.RunTestsAsync();
          break;
        case "ON_DESTROY_SERVER":
          break;
        case "ON_DESTROY_SERVER_AFTER":
          break;
      }
    }

    [UnmanagedCallersOnly]
    private static void MainLoopHandler(ulong frame)
    {
      TestSyncContext.Update();
      OnMainLoop?.Invoke(frame);
    }

    private static void PrelinkLibraryImports()
    {
      Console.WriteLine("Prelinking library import methods");
      try
      {
        Marshal.PrelinkAll(typeof(NWNXAPI));
        Console.WriteLine("Prelinking complete");
      }
      catch (TypeInitializationException)
      {
        Console.Error.WriteLine("The NWNX_DotNET plugin could not be found.");
        throw;
      }
      catch (Exception)
      {
        Console.Error.WriteLine("The NWNX_DotNET plugin could not be loaded.");
        throw;
      }
    }
  }
}
