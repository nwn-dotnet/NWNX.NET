using System;
using NUnit.Framework;
using NWNX.NET.Tests.Constants;
using NWNX.NET.Tests.Native;

namespace NWNX.NET.Tests.EngineStructures
{
  [TestFixture(Category = "Engine Structures")]
  public class CassowaryTests
  {
    [Test]
    public void CassowaryTest()
    {
      NWNXAPI.CallBuiltIn(VMFunctions.GetModule);
      uint moduleObjectId = NWNXAPI.StackPopObject();

      NWNXAPI.StackPushString(string.Empty);
      NWNXAPI.StackPushObject(moduleObjectId);
      NWNXAPI.CallBuiltIn(VMFunctions.GetLocalCassowary);

      using EngineStructure cassowary = new EngineStructure(EngineStructureType.Cassowary, NWNXAPI.StackPopGameDefinedStructure(EngineStructureType.Cassowary));

      Assert.That(cassowary.Pointer, Is.Not.EqualTo(IntPtr.Zero));

      AddConstraint("middle == (left + right) / 2");
      AddConstraint("right == left + 10");
      AddConstraint("right <= 100");
      AddConstraint("left >= 0");

      AssertCorrectValue("left", 90f);
      AssertCorrectValue("middle", 95f);
      AssertCorrectValue("right", 100f);
      return;

      void AddConstraint(string constraint)
      {
        const float strength = 1001001000.0f;

        NWNXAPI.StackPushFloat(strength);
        NWNXAPI.StackPushString(constraint);
        NWNXAPI.StackPushGameDefinedStructure(EngineStructureType.Cassowary, cassowary);
        NWNXAPI.CallBuiltIn(VMFunctions.CassowaryConstrain);

        string? error = NWNXAPI.StackPopString();
        Assert.That(error, Is.Empty);
      }

      void AssertCorrectValue(string key, float expectedValue)
      {
        NWNXAPI.StackPushString(key);
        NWNXAPI.StackPushGameDefinedStructure(EngineStructureType.Cassowary, cassowary);
        NWNXAPI.CallBuiltIn(VMFunctions.CassowaryGetValue);

        float actualValue = NWNXAPI.StackPopFloat();
        Assert.That(actualValue, Is.EqualTo(expectedValue));
      }
    }
  }
}
