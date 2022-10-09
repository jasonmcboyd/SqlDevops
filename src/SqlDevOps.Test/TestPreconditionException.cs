using System;

namespace SqlDevOps.Test
{
  public class TestPreconditionException : Exception
  {
    public TestPreconditionException(string message, Exception innerException) : base(message, innerException)
    {
    }
  }
}
