using System;
using NUnit.Framework;
using NWNX.NET.Tests.Constants;
using NWNX.NET.Tests.Native;

namespace NWNX.NET.Tests.EngineStructures
{
  [TestFixture(Category = "Engine Structures")]
  public sealed class JsonTests
  {
    [Test]
    public void JsonTest()
    {
      const string jsonString = "[1,2,3]";

      NWNXAPI.StackPushString(jsonString);
      NWNXAPI.CallBuiltIn(VMFunctions.JsonParse);
      using EngineStructure jsonArray = new EngineStructure(EngineStructureType.Json, NWNXAPI.StackPopGameDefinedStructure(EngineStructureType.Json));

      Assert.That(jsonArray.Pointer, Is.Not.EqualTo(IntPtr.Zero));

      NWNXAPI.StackPushInteger(1);
      NWNXAPI.StackPushGameDefinedStructure(EngineStructureType.Json, jsonArray);
      NWNXAPI.CallBuiltIn(VMFunctions.JsonArrayGet);
      using EngineStructure jsonArrayValue2 = new EngineStructure(EngineStructureType.Json, NWNXAPI.StackPopGameDefinedStructure(EngineStructureType.Json));

      NWNXAPI.StackPushGameDefinedStructure(EngineStructureType.Json, jsonArrayValue2);
      NWNXAPI.CallBuiltIn(VMFunctions.JsonGetInt);
      int value = NWNXAPI.StackPopInteger();

      Assert.That(value, Is.EqualTo(2));
    }
  }
}
