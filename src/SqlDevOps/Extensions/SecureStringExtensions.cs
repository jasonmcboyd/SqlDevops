using System;
using System.Runtime.InteropServices;
using System.Security;

namespace SqlDevOps.Extensions
{
  public static class SecureStringExtensions
  {
    public static string ToPlainTextString(this SecureString value)
    {
      var pointer = IntPtr.Zero;
      try
      {
        pointer = Marshal.SecureStringToGlobalAllocUnicode(value);
        return Marshal.PtrToStringUni(pointer);
      }
      finally
      {
        Marshal.ZeroFreeGlobalAllocUnicode(pointer);
      }
    }
  }
}
