using DotNet.Testcontainers.Containers;

using SqlDevOps.Test.Extensions;
using SqlDevOps.Test.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SqlDevOps.Test.Sql
{
  internal class TestDatabase : TestDatabaseObjectBase
  {
    public TestDatabase() : base(RandomStringBuilder.GetString(10))
    {
    }

    public List<TestDatabaseTable> Tables { get; } = new List<TestDatabaseTable>();
    public List<TestDatabaseStoredProcedure> StoredProcedures { get; } = new List<TestDatabaseStoredProcedure>();

    public TestDatabase WithTable()
    {
      Tables.Add(new TestDatabaseTable());
      return this;
    }

    public TestDatabase WithValidStoredProcedure()
    {
      StoredProcedures.Add(new TestDatabaseStoredProcedure(true));
      return this;
    }

    public TestDatabase WithInvalidStoredProcedure()
    {
      StoredProcedures.Add(new TestDatabaseStoredProcedure(false));
      return this;
    }

    public IEnumerable<TestDatabaseObjectBase> GetDatabaseObjects()
    {
      foreach (var obj in Tables)
        yield return obj;

      foreach (var obj in StoredProcedures)
        yield return obj;
    }

    public override string Definition => $"create database {Name}";

    public async Task CreateAsync(string connectionString)
    {
      SqlConnection connection = null;
      SqlCommand command = null;

      await using (connection = new SqlConnection(connectionString))
      await using (command = connection.CreateCommand())
      {
        connection.Open();

        command.CommandText = Definition;
        await command.ExecuteNonQueryAsync();
      }

      var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
      connectionStringBuilder.InitialCatalog = Name;
      connectionString = connectionStringBuilder.ToString();

      await using (connection = new SqlConnection(connectionString))
      await using (command = connection.CreateCommand())
      {
        connection.Open();

        foreach (var databaseObject in GetDatabaseObjects())
        {
          command.CommandText = databaseObject.Definition;
          await command.ExecuteNonQueryAsync();
        }
      }
    }
  }
}
