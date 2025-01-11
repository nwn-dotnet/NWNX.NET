using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using NWNX.NET.Tests.Async;

namespace NWNX.NET.Tests.Events
{
  [TestFixture(Category = "Events")]
  public sealed class MainLoopEventTests
  {
    private const int FrameCount = 60;

    private bool testRunning;

    private readonly List<ulong> loopFrames = [];
    private readonly List<int> loopThreads = [];

    [SetUp]
    public void Setup()
    {
      Program.OnMainLoop += OnMainLoop;
    }

    [Test]
    [Timeout(5000)]
    public async Task MainLoopTest()
    {
      int mainThreadId = Thread.CurrentThread.ManagedThreadId;
      testRunning = true;

      CancellationTokenSource cts = new CancellationTokenSource();
      cts.CancelAfter(TimeSpan.FromSeconds(5));

      await NwTask.WaitUntil(() => !testRunning, cts.Token);

      Assert.That(loopFrames.Count, Is.EqualTo(FrameCount));
      Assert.That(loopThreads.Count, Is.EqualTo(FrameCount));

      ulong[] expectedLoopFrames = Enumerable.Range((int)loopFrames[0], FrameCount).Select(i => (ulong)i).ToArray();
      int[] expectedLoopThreads = Enumerable.Repeat(mainThreadId, FrameCount).ToArray();

      Assert.That(loopFrames, Is.EqualTo(expectedLoopFrames));
      Assert.That(loopThreads, Is.EqualTo(expectedLoopThreads));
    }

    private void OnMainLoop(ulong frame)
    {
      if (!testRunning)
      {
        return;
      }

      if (loopFrames.Count < FrameCount)
      {
        loopFrames.Add(frame);
        loopThreads.Add(Thread.CurrentThread.ManagedThreadId);
      }
      else
      {
        testRunning = false;
      }
    }

    [TearDown]
    public void TearDown()
    {
      Program.OnMainLoop -= OnMainLoop;
      loopFrames.Clear();
      loopThreads.Clear();
    }
  }
}
