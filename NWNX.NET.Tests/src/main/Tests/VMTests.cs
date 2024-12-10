using System.Collections.Generic;
using NUnit.Framework;
using NWNX.NET.Tests.Constants;

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
  }
}
