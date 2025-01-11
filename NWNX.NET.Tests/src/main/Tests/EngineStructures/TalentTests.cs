using NUnit.Framework;
using NWNX.NET.Tests.Constants;
using NWNX.NET.Tests.Native;

namespace NWNX.NET.Tests.EngineStructures
{
  [TestFixture(Category = "Engine Structures")]
  public sealed class TalentTests
  {
    private const int TalentTypeSpell = 0;
    private const int TalentTypeFeat = 1;
    private const int TalentTypeSkill = 2;

    [Test]
    [TestCase(10, 1)]
    [TestCase(1, 1)]
    [TestCase(-30, 1)]
    [TestCase(int.MaxValue, 1)]
    public void SpellTalentTests(int spellId, int expectedValid)
    {
      NWNXAPI.StackPushInteger(spellId);
      NWNXAPI.CallBuiltIn(VMFunctions.TalentSpell);

      using EngineStructure talent = new EngineStructure(EngineStructureType.Talent, NWNXAPI.StackPopGameDefinedStructure(EngineStructureType.Talent));
      CheckTalentValid(talent, TalentTypeSpell, expectedValid);
    }

    [Test]
    [TestCase(10, 1)]
    [TestCase(1, 1)]
    [TestCase(-30, 1)]
    [TestCase(int.MaxValue, 1)]
    public void SkillTalentTests(int skillId, int expectedValid)
    {
      NWNXAPI.StackPushInteger(skillId);
      NWNXAPI.CallBuiltIn(VMFunctions.TalentSkill);

      using EngineStructure talent = new EngineStructure(EngineStructureType.Talent, NWNXAPI.StackPopGameDefinedStructure(EngineStructureType.Talent));
      CheckTalentValid(talent, TalentTypeSkill, expectedValid);
    }

    [Test]
    [TestCase(10, 1)]
    [TestCase(1, 1)]
    [TestCase(-30, 1)]
    [TestCase(int.MaxValue, 1)]
    public void FeatTalentTests(int featId, int expectedValid)
    {
      NWNXAPI.StackPushInteger(featId);
      NWNXAPI.CallBuiltIn(VMFunctions.TalentFeat);

      using EngineStructure talent = new EngineStructure(EngineStructureType.Talent, NWNXAPI.StackPopGameDefinedStructure(EngineStructureType.Talent));
      CheckTalentValid(talent, TalentTypeFeat, expectedValid);
    }

    private static void CheckTalentValid(EngineStructure talent, int expectedTalentType, int expectedValid)
    {
      NWNXAPI.StackPushGameDefinedStructure(EngineStructureType.Talent, talent);
      NWNXAPI.CallBuiltIn(VMFunctions.GetIsTalentValid);

      int valid = NWNXAPI.StackPopInteger();
      Assert.That(valid, Is.EqualTo(expectedValid));

      NWNXAPI.StackPushGameDefinedStructure(EngineStructureType.Talent, talent);
      NWNXAPI.CallBuiltIn(VMFunctions.GetTypeFromTalent);

      int talentType = NWNXAPI.StackPopInteger();
      Assert.That(talentType, Is.EqualTo(expectedTalentType));
    }
  }
}
