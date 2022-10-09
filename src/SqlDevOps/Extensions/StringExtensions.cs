using System.Security;

namespace SqlDevOps.Extensions
{
  public static class StringExtensions
  {
    public static SecureString ToSecureString(this string value)
    {
      var result = new SecureString();
      
      for (int i = 0; i < value.Length; i++)
        result.AppendChar(value[i]);
      
      return result;
    }
  }
}
