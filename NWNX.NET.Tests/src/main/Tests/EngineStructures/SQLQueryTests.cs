using System;
using NUnit.Framework;
using NWNX.NET.Tests.Constants;
using NWNX.NET.Tests.Native;

namespace NWNX.NET.Tests.EngineStructures
{
  [TestFixture(Category = "Engine Structures")]
  public class SQLQueryTests
  {
    [Test]
    public void SQLQueryEngineStructureTest()
    {
      NWNXAPI.CallBuiltIn(VMFunctions.GetModule);
      uint moduleObjectId = NWNXAPI.StackPopObject();

      const string query = "SELECT * FROM 'test'";

      NWNXAPI.StackPushString(query);
      NWNXAPI.StackPushObject(moduleObjectId);
      NWNXAPI.CallBuiltIn(VMFunctions.SqlPrepareQueryObject);

      using EngineStructure sqlQuery = new EngineStructure(EngineStructureType.SqlQuery, NWNXAPI.StackPopGameDefinedStructure(EngineStructureType.SqlQuery));

      Assert.That(sqlQuery.Pointer, Is.Not.EqualTo(IntPtr.Zero));
    }
  }
}
