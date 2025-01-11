using System;
using NUnit.Framework;
using NWNX.NET.Tests.Constants;
using NWNX.NET.Tests.Native;

namespace NWNX.NET.Tests.EngineStructures
{
  [TestFixture(Category = "Engine Structures")]
  public sealed class EffectTests
  {
    [Test]
    public void EffectNegativeLevelTest()
    {
      const string expectedEffectTag = "effect_tag";
      const int expectedNumLevels = 5;
      const int expectedHpBonus = 10;

      NWNXAPI.StackPushInteger(expectedHpBonus);
      NWNXAPI.StackPushInteger(expectedNumLevels);
      NWNXAPI.CallBuiltIn(VMFunctions.EffectNegativeLevel);

      using EngineStructure effect = new EngineStructure(EngineStructureType.Effect, NWNXAPI.StackPopGameDefinedStructure(EngineStructureType.Effect));

      Assert.That(effect.Pointer, Is.Not.EqualTo(IntPtr.Zero));

      NWNXAPI.StackPushString(expectedEffectTag);
      NWNXAPI.StackPushGameDefinedStructure(EngineStructureType.Effect, effect);
      NWNXAPI.CallBuiltIn(VMFunctions.TagEffect);

      using EngineStructure taggedEffect = new EngineStructure(EngineStructureType.Effect, NWNXAPI.StackPopGameDefinedStructure(EngineStructureType.Effect));

      Assert.That(effect.Pointer, Is.Not.EqualTo(taggedEffect.Pointer));
      Assert.That(taggedEffect.Pointer, Is.Not.EqualTo(IntPtr.Zero));

      NWNXAPI.StackPushGameDefinedStructure(EngineStructureType.Effect, taggedEffect);
      NWNXAPI.CallBuiltIn(VMFunctions.GetEffectTag);

      string? effectTag = NWNXAPI.StackPopString();
      Assert.That(effectTag, Is.EqualTo(expectedEffectTag));

      NWNXAPI.StackPushInteger(0);
      NWNXAPI.StackPushGameDefinedStructure(EngineStructureType.Effect, taggedEffect);
      NWNXAPI.CallBuiltIn(VMFunctions.GetEffectInteger);

      int numLevels = NWNXAPI.StackPopInteger();
      Assert.That(numLevels, Is.EqualTo(expectedNumLevels));

      NWNXAPI.StackPushInteger(1);
      NWNXAPI.StackPushGameDefinedStructure(EngineStructureType.Effect, taggedEffect);
      NWNXAPI.CallBuiltIn(VMFunctions.GetEffectInteger);

      int hpBonus = NWNXAPI.StackPopInteger();
      Assert.That(hpBonus, Is.EqualTo(expectedHpBonus));
    }
  }
}
