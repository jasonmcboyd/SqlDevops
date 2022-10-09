using DotNet.Testcontainers.Containers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SqlDevOps.Test.Utilities;

using System;
using System.Threading.Tasks;

namespace SqlDevOps.Test
{
  [TestClass]
  public class Initialize
  {
    public static MsSqlTestcontainer Container { get; set; }

    [AssemblyInitialize]
    public static async Task AssemblyInitialize(TestContext context)
    {
      Console.WriteLine("AssemblyInitialize");
      Container = await TestDatabaseContainerFactory.CreateContainerAsync();
    }

    [AssemblyCleanup]
    public static async Task AssemblyCleanup()
    {
      if (Container is null)
        return;

      await Container.DisposeAsync();
    }
  }
}
