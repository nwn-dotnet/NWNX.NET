using NUnit.Framework;

namespace NWNX.NET.Tests
{
  [TestFixture(Category = "VM Tests")]
  public sealed class VMTests
  {
    [Test]
    public void GetModuleNameTest()
    {
      NWNXAPI.CallBuiltIn(561);
      string? modName = NWNXAPI.StackPopString();

      Assert.That(modName, Is.EqualTo("demo"));
    }
  }
}
