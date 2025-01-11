using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using NWNX.NET.Native;
using NWNX.NET.Tests.Async;
using NWNX.NET.Tests.Constants;

namespace NWNX.NET.Tests.Events
{
  [TestFixture(Category = "Events")]
  public sealed class RunScriptEventTests
  {
    private const string EventScriptName = "dotnet_test";

    private static string? triggeredScriptName;
    private static uint triggeredScriptOid;
    private static int triggeredThreadId;
    private static bool scriptTriggered;

    [SetUp]
    public unsafe void Setup()
    {
      NWNXAPI.RegisterRunScriptHandler(&RunScriptHandler);
    }

    [Test]
    [Timeout(5000)]
    public async Task RunScriptTest()
    {
      int mainThreadId = Thread.CurrentThread.ManagedThreadId;

      NWNXAPI.CallBuiltIn(VMFunctions.GetFirstArea);
      uint scriptContextObj = NWNXAPI.StackPopObject();

      NWNXAPI.StackPushObject(scriptContextObj);
      NWNXAPI.StackPushString(EventScriptName);
      NWNXAPI.CallBuiltIn(VMFunctions.ExecuteScript);

      CancellationTokenSource cts = new CancellationTokenSource();
      cts.CancelAfter(TimeSpan.FromSeconds(5));

      await NwTask.WaitUntil(() => scriptTriggered, cts.Token);

      Assert.That(scriptTriggered, Is.True);

      Assert.That(triggeredScriptName, Is.EqualTo(EventScriptName));
      Warn.Unless(triggeredScriptOid, Is.EqualTo(scriptContextObj));
      Assert.That(triggeredThreadId, Is.EqualTo(mainThreadId));
    }

    [UnmanagedCallersOnly]
    private static int RunScriptHandler(IntPtr pScript, uint oidSelf)
    {
      if (scriptTriggered)
      {
        return -1;
      }

      triggeredScriptName = pScript.ReadNullTerminatedString();
      triggeredScriptOid = oidSelf;
      triggeredThreadId = Thread.CurrentThread.ManagedThreadId;
      scriptTriggered = true;

      return -1;
    }

    [TearDown]
    public unsafe void TearDown()
    {
      NWNXAPI.RegisterRunScriptHandler(null);
    }
  }
}
