using System.Linq;
using System.Management.Automation;

namespace SqlDevOps.Test.Utilities
{
  public static class CmdletUtilities
  {
    public static string GetName<T>()
      where T : Cmdlet
    {
      var cmdletAttribute = typeof(T).CustomAttributes.First(x => x.AttributeType == typeof(CmdletAttribute));
      return $"{cmdletAttribute.ConstructorArguments[0].Value}-{cmdletAttribute.ConstructorArguments[1].Value}";
    }
  }
}
