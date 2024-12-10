using System.Numerics;
using NUnit.Framework;
using NWNX.NET.Tests.Constants;
using NWNX.NET.Tests.Native;

namespace NWNX.NET.Tests.EngineStructures
{
  [TestFixture(Category = "VM Tests")]
  public sealed class LocationTests
  {
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
