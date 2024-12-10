using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using NUnit.Framework;
using NWNX.NET.Native;
using NWNX.NET.Tests.Async;
using NWNX.NET.Tests.Constants;
using NWNX.NET.Tests.Native;

namespace NWNX.NET.Tests.EngineStructures
{
  [TestFixture(Category = "VM Tests")]
  public sealed class EventTests
  {
    private const int UserDefinedEventNumber = 12321;
    private const int UserDefinedEventType = 3001;
    private const uint ModuleObjectId = 0;
    private const string EventScriptName = "event_test";

    private static int triggeredEventNumber;

    [SetUp]
    public unsafe void Setup()
    {
      triggeredEventNumber = 0;

      NWNXAPI.RegisterRunScriptHandler(&RunScriptHandler);

      NWNXAPI.StackPushString(EventScriptName);
      NWNXAPI.StackPushInteger(UserDefinedEventType);
      NWNXAPI.StackPushObject(ModuleObjectId);
      NWNXAPI.CallBuiltIn(VMFunctions.SetEventScript);
    }

    [Test]
    public async Task EventTest()
    {
      NWNXAPI.StackPushInteger(UserDefinedEventNumber);
      NWNXAPI.CallBuiltIn(VMFunctions.EventUserDefined);

      using EngineStructure eventStruct = new EngineStructure(EngineStructureType.Event, NWNXAPI.StackPopGameDefinedStructure(EngineStructureType.Event));
      Assert.That(eventStruct.Pointer, Is.Not.EqualTo(IntPtr.Zero));

      NWNXAPI.StackPushGameDefinedStructure(EngineStructureType.Event, eventStruct);
      NWNXAPI.StackPushObject(ModuleObjectId);
      NWNXAPI.CallBuiltIn(VMFunctions.SignalEvent);

      await NwTask.NextFrame();

      Assert.That(triggeredEventNumber, Is.EqualTo(UserDefinedEventNumber));
    }

    [TearDown]
    public unsafe void TearDown()
    {
      NWNXAPI.RegisterRunScriptHandler(null);

      NWNXAPI.StackPushString(string.Empty);
      NWNXAPI.StackPushInteger(UserDefinedEventType);
      NWNXAPI.StackPushObject(ModuleObjectId);
      NWNXAPI.CallBuiltIn(VMFunctions.SetEventScript);
    }

    [UnmanagedCallersOnly]
    internal static int RunScriptHandler(IntPtr pScript, uint oidSelf)
    {
      string? script = pScript.ReadNullTerminatedString();

      if (script == EventScriptName)
      {
        NWNXAPI.CallBuiltIn(VMFunctions.GetUserDefinedEventNumber);
        triggeredEventNumber = NWNXAPI.StackPopInteger();
      }

      return -1;
    }
  }
}
