using System;

namespace SqlDevOps.Test.Utilities
{
  public static class TestHelper
  {
    public static void Assume(Action assertion, string message)
    {
      try
      {
        assertion.Invoke();
      }
      catch (Exception e)
      {
        throw new TestPreconditionException(message, e);
      }
    }
  }
}
