using System;
using System.IO;
using System.Threading;
using NUnit.Framework.Internal.Commands;
using NUnitLite;

namespace NWNX.NET.Tests
{
  public sealed class TestRunner
  {
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
      Environment.Exit(result);
    }

    private string[] GetRunnerArguments()
    {
      string outputPath = Path.Combine(Environment.CurrentDirectory, "results");
      return ["--mainthread", $"--work={outputPath}"];
    }
  }
}
