namespace SqlDevOps.Test.Sql
{
  internal abstract class TestDatabaseObjectBase
  {
    public TestDatabaseObjectBase(string name)
    {
      Name = name;
    }

    public string Name { get; }
    public abstract string Definition { get; }
  }
}
