using NUnit.Framework;

namespace NWNX.NET.Tests
{
  [TestFixture(Category = "NWNX VM Tests")]
  public sealed class NWNXVMTests
  {
    [Test]
    [TestCase("NWNX_Core", 1)]
    [TestCase("NWNX_DotNET", 1)]
    [TestCase("NWNX_SWIG_DotNET", 1)]
    [TestCase("NWNX_MissingPlugin", 0)]
    [TestCase("", 0)]
    [TestCase(null, 0, Ignore = "NWNX throws a std::logic_error")]
    public void PluginExistsTest(string plugin, int expectedResult)
    {
      NWNXAPI.NWNXSetFunction("NWNX_Core", "PluginExists");
      NWNXAPI.NWNXPushString(plugin);
      NWNXAPI.NWNXCallFunction();

      int result = NWNXAPI.NWNXPopInt();

      Assert.That(result, Is.EqualTo(expectedResult));
    }
  }
}
