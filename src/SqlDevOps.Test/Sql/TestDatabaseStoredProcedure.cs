using SqlDevOps.Test.Utilities;

namespace SqlDevOps.Test.Sql
{
  internal class TestDatabaseStoredProcedure : TestDatabaseObjectBase
  {
    public TestDatabaseStoredProcedure(bool isValid) : base(RandomStringBuilder.GetString(10))
    {
      IsValid = isValid;
    }

    public bool IsValid { get; }
    public override string Definition
    {
      get
      {
        if (IsValid)
        {
          return $@"
            create procedure {Name}
            as
            begin
              select 1
            end";
        }
        else
        {
          return $@"
            create procedure {Name}
            as
            begin
              select *
              from [DatabaseThatDoesNotExist].[dbo].[TableThatDoesNotExist]
            end";
        }
      }
    }
  }
}
