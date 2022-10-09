using SqlDevOps.Test.Utilities;

namespace SqlDevOps.Test.Sql
{
  internal class TestDatabaseTable : TestDatabaseObjectBase
  {
    public TestDatabaseTable() : base(RandomStringBuilder.GetString(10))
    {
    }

    public override string Definition => $@"
      create table {Name} (
        Id int identity(1, 1) not null,
        constraint [PK_{Name}] primary key clustered ([Id])
      )";
  }
}
