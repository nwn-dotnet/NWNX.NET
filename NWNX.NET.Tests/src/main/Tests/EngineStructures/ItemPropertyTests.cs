using System;
using NUnit.Framework;
using NWNX.NET.Tests.Constants;
using NWNX.NET.Tests.Native;

namespace NWNX.NET.Tests.EngineStructures
{
  [TestFixture(Category = "Engine Structures")]
  public sealed class ItemPropertyTests
  {
    [Test]
    public void ItemPropertyHasteTest()
    {
      const string expectedPropertyTag = "property_tag";

      NWNXAPI.CallBuiltIn(VMFunctions.ItemPropertyHaste);

      using EngineStructure itemProperty = new EngineStructure(EngineStructureType.ItemProperty, NWNXAPI.StackPopGameDefinedStructure(EngineStructureType.ItemProperty));

      Assert.That(itemProperty.Pointer, Is.Not.EqualTo(IntPtr.Zero));

      NWNXAPI.StackPushString(expectedPropertyTag);
      NWNXAPI.StackPushGameDefinedStructure(EngineStructureType.ItemProperty, itemProperty);
      NWNXAPI.CallBuiltIn(VMFunctions.TagItemProperty);

      using EngineStructure taggedItemProperty = new EngineStructure(EngineStructureType.ItemProperty, NWNXAPI.StackPopGameDefinedStructure(EngineStructureType.ItemProperty));

      Assert.That(itemProperty.Pointer, Is.Not.EqualTo(taggedItemProperty.Pointer));
      Assert.That(taggedItemProperty.Pointer, Is.Not.EqualTo(IntPtr.Zero));

      NWNXAPI.StackPushGameDefinedStructure(EngineStructureType.ItemProperty, taggedItemProperty);
      NWNXAPI.CallBuiltIn(VMFunctions.GetItemPropertyTag);

      string? itemPropertyTag = NWNXAPI.StackPopString();
      Assert.That(itemPropertyTag, Is.EqualTo(expectedPropertyTag));
    }
  }
}
