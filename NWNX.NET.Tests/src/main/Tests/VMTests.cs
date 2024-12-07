using System.Collections.Generic;
using System.Numerics;
using NUnit.Framework;
using NWNX.NET.Tests.Constants;
using NWNX.NET.Tests.Native;

namespace NWNX.NET.Tests
{
  [TestFixture(Category = "VM Tests")]
  public sealed class VMTests
  {
    [Test]
    public void GetModuleNameTest()
    {
      NWNXAPI.CallBuiltIn(VMFunctions.GetModuleName);
      string? modName = NWNXAPI.StackPopString();

      Assert.That(modName, Is.EqualTo("demo"));
    }

    [Test]
    [TestCase(1, 1)]
    [TestCase(0, 0)]
    [TestCase(-1, 1)]
    [TestCase(-int.MaxValue, int.MaxValue)]
    public void AbsTest(int value, int expectedValue)
    {
      NWNXAPI.StackPushInteger(value);
      NWNXAPI.CallBuiltIn(VMFunctions.abs);
      int result = NWNXAPI.StackPopInteger();

      Assert.That(result, Is.EqualTo(expectedValue));
    }

    [Test]
    [TestCase(1f, 1f)]
    [TestCase(0f, 0f)]
    [TestCase(-1f, 1f)]
    [TestCase(-float.MaxValue, float.MaxValue)]
    public void FAbsTest(float value, float expectedValue)
    {
      NWNXAPI.StackPushFloat(value);
      NWNXAPI.CallBuiltIn(VMFunctions.fabs);
      float result = NWNXAPI.StackPopFloat();

      Assert.That(result, Is.EqualTo(expectedValue));
    }

    [Test]
    [TestCase("example_string", 0, 7, "example")]
    [TestCase("test_string", 5, 6, "string")]
    [TestCase("", 0, 0, "")]
    [TestCase(null, 0, 5, "")]
    public void SubStringTest(string str, int start, int count, string expectedValue)
    {
      NWNXAPI.StackPushInteger(count);
      NWNXAPI.StackPushInteger(start);
      NWNXAPI.StackPushString(str);
      NWNXAPI.CallBuiltIn(VMFunctions.SubString);

      string? result = NWNXAPI.StackPopString();
      Assert.That(result, Is.EqualTo(expectedValue));
    }

    [Test]
    public void GetFirstAreaTest()
    {
      NWNXAPI.CallBuiltIn(VMFunctions.GetFirstArea);
      uint result = NWNXAPI.StackPopObject();

      Assert.That(result, Is.Not.EqualTo(VMConstants.ObjectInvalid));

      NWNXAPI.StackPushInteger(0);
      NWNXAPI.StackPushObject(result);
      NWNXAPI.CallBuiltIn(VMFunctions.GetName);

      string? name = NWNXAPI.StackPopString();

      Assert.That(name, Is.EqualTo("Start"));
    }

    [Test]
    public void AreaObjectIteratorTest()
    {
      NWNXAPI.CallBuiltIn(VMFunctions.GetFirstArea);
      uint area = NWNXAPI.StackPopObject();

      NWNXAPI.StackPushInteger(VMConstants.ObjectTypeAll);
      NWNXAPI.StackPushObject(area);
      NWNXAPI.CallBuiltIn(VMFunctions.GetFirstObjectInArea);

      uint gameObject = NWNXAPI.StackPopObject();
      HashSet<uint> gameObjects =
      [
        gameObject,
      ];

      while (gameObject != VMConstants.ObjectInvalid)
      {
        NWNXAPI.StackPushInteger(VMConstants.ObjectTypeAll);
        NWNXAPI.StackPushObject(area);
        NWNXAPI.CallBuiltIn(VMFunctions.GetNextObjectInArea);

        gameObject = NWNXAPI.StackPopObject();
        Assert.That(gameObjects.Add(gameObject), Is.True, $"Game object iterator returned a duplicate object: '{gameObject}'");
      }
    }

    [Test]
    [TestCase(0f, 0f, 0f, 0f)]
    [TestCase(10f, 2f, 20f, 180f)]
    [TestCase(-150f, 200f, 10f, 90f)]
    [TestCase(-float.MaxValue, float.MaxValue, float.MaxValue, 10f)]
    public void LocationEngineStructureTest(float x, float y, float z, float facing)
    {
      NWNXAPI.CallBuiltIn(VMFunctions.GetFirstArea);
      uint area = NWNXAPI.StackPopObject();
      Vector3 position = new Vector3(x, y, z);

      NWNXAPI.StackPushFloat(facing);
      NWNXAPI.StackPushVector(position);
      NWNXAPI.StackPushObject(area);
      NWNXAPI.CallBuiltIn(VMFunctions.Location);

      using EngineStructure location = new EngineStructure(EngineStructureType.Location, NWNXAPI.StackPopGameDefinedStructure(EngineStructureType.Location));

      NWNXAPI.StackPushGameDefinedStructure(EngineStructureType.Location, location);
      NWNXAPI.CallBuiltIn(VMFunctions.GetAreaFromLocation);
      uint locationArea = NWNXAPI.StackPopObject();

      Assert.That(locationArea, Is.EqualTo(area), "Location area does not match source area");

      NWNXAPI.StackPushGameDefinedStructure(EngineStructureType.Location, location);
      NWNXAPI.CallBuiltIn(VMFunctions.GetPositionFromLocation);
      Vector3 locationPosition = NWNXAPI.StackPopVector();

      Assert.That(locationPosition, Is.EqualTo(position), "Location position does not match source position");

      NWNXAPI.StackPushGameDefinedStructure(EngineStructureType.Location, location);
      NWNXAPI.CallBuiltIn(VMFunctions.GetFacingFromLocation);
      float locationFacing = NWNXAPI.StackPopFloat();

      Assert.That(locationFacing, Is.EqualTo(facing), "Location facing does not match source facing.");
    }
  }
}
