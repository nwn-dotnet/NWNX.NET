using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using NUnit.Framework.Internal.Commands;
using NUnitLite;

namespace NWNX.NET.Tests
{
  public sealed unsafe class TestRunner
  {
    private static readonly string TestResultPath = Path.GetFullPath(Environment.GetEnvironmentVariable("NWNX_DOTNET_TEST_RESULT_PATH") ?? "results", Environment.CurrentDirectory);
    private static readonly int* ExitProgram = (int*)NativeLibrary.GetExport(NativeLibrary.GetMainProgramHandle(), "g_bExitProgram");

    private readonly TextRunner testRunner;
    private Thread? testWorkerThread;

    public TestRunner(MainThreadSynchronizationContext testSyncContext)
    {
      TestCommand.DefaultSynchronizationContext = testSyncContext;
      testRunner = new TextRunner(typeof(Program).Assembly);
    }

    public void RunTestsAsync()
    {
      testWorkerThread = new Thread(ExecuteTestRun);
      testWorkerThread.Start();
    }

    private void ExecuteTestRun()
    {
      int result = testRunner.Execute(GetRunnerArguments());
      string output = $"Test runner result: {result}";

      Console.WriteLine(output);

      if (result == 0)
      {
        *ExitProgram = 1;
      }
      else
      {
        Environment.FailFast(output);
      }
    }

    private string[] GetRunnerArguments()
    {
      return ["--mainthread", $"--work={TestResultPath}"];
    }
  }
}
