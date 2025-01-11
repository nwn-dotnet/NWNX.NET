using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using NWNX.NET.Tests.Async;
using NWNX.NET.Tests.Constants;

namespace NWNX.NET.Tests.Events
{
  [TestFixture(Category = "Events")]
  public sealed class ClosureEventTests
  {
    private static ulong nextEventId;

    private static int closureThreadId;
    private static ulong closureEventId;
    private static uint closureObjectId;
    private static bool closureTriggered;

    [SetUp]
    public unsafe void Setup()
    {
      NWNXAPI.RegisterClosureHandler(&OnClosure);
    }

    [Test]
    public async Task DelayCommandTest()
    {
      int expectedThreadId = Thread.CurrentThread.ManagedThreadId;

      NWNXAPI.CallBuiltIn(VMFunctions.GetFirstArea);
      uint expectedObjectId = NWNXAPI.StackPopObject();

      ulong expectedEventId = nextEventId;
      NWNXAPI.ClosureDelayCommand(expectedObjectId, 1.0f, nextEventId++);

      CancellationTokenSource cts = new CancellationTokenSource();
      cts.CancelAfter(TimeSpan.FromSeconds(5));

      await NwTask.WaitUntil(() => closureTriggered, cts.Token);

      Assert.That(closureTriggered, Is.True);
      Assert.That(closureObjectId, Is.EqualTo(expectedObjectId));
      Assert.That(closureEventId, Is.EqualTo(expectedEventId));
      Assert.That(closureThreadId, Is.EqualTo(expectedThreadId));
    }

    [Test]
    public async Task AssignCommandTest()
    {
      int expectedThreadId = Thread.CurrentThread.ManagedThreadId;

      NWNXAPI.CallBuiltIn(VMFunctions.GetFirstArea);
      uint expectedObjectId = NWNXAPI.StackPopObject();

      ulong expectedEventId = nextEventId;
      NWNXAPI.ClosureAssignCommand(expectedObjectId, nextEventId++);

      CancellationTokenSource cts = new CancellationTokenSource();
      cts.CancelAfter(TimeSpan.FromSeconds(5));

      await NwTask.WaitUntil(() => closureTriggered, cts.Token);

      Assert.That(closureTriggered, Is.True);
      Assert.That(closureObjectId, Is.EqualTo(expectedObjectId));
      Assert.That(closureEventId, Is.EqualTo(expectedEventId));
      Assert.That(closureThreadId, Is.EqualTo(expectedThreadId));
    }

    [Test]
    public async Task ActionDoCommandTest()
    {
      int expectedThreadId = Thread.CurrentThread.ManagedThreadId;

      NWNXAPI.CallBuiltIn(VMFunctions.GetFirstArea);
      uint areaId = NWNXAPI.StackPopObject();

      NWNXAPI.StackPushInteger(VMConstants.ObjectTypeAll);
      NWNXAPI.StackPushObject(areaId);
      NWNXAPI.CallBuiltIn(VMFunctions.GetFirstObjectInArea);
      uint expectedObjectId = NWNXAPI.StackPopObject();

      ulong expectedEventId = nextEventId;
      NWNXAPI.ClosureActionDoCommand(expectedObjectId, nextEventId++);

      CancellationTokenSource cts = new CancellationTokenSource();
      cts.CancelAfter(TimeSpan.FromSeconds(5));

      await NwTask.WaitUntil(() => closureTriggered, cts.Token);

      Assert.That(closureTriggered, Is.True);
      Assert.That(closureObjectId, Is.EqualTo(expectedObjectId));
      Assert.That(closureEventId, Is.EqualTo(expectedEventId));
      Assert.That(closureThreadId, Is.EqualTo(expectedThreadId));
    }

    [UnmanagedCallersOnly]
    private static void OnClosure(ulong eid, uint oidSelf)
    {
      closureThreadId = Thread.CurrentThread.ManagedThreadId;
      closureEventId = eid;
      closureObjectId = oidSelf;
      closureTriggered = true;
    }

    [TearDown]
    public unsafe void TearDown()
    {
      NWNXAPI.RegisterClosureHandler(null);
      closureThreadId = 0;
      closureEventId = 0;
      closureObjectId = 0;
      closureTriggered = false;
    }
  }
}
