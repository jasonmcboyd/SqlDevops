using System;
using System.Text;

namespace SqlDevOps.Test.Utilities
{
  internal static class RandomStringBuilder
  {
    private const string ALPHABET = "abcdefghijklmnopqrstuvwxyz";
    private static Random Random = new Random(0);

    public static string GetString(int length)
    {
      var builder = new StringBuilder();

      for (int i = 0; i < length; i++)
        builder.Append(ALPHABET[Random.Next(ALPHABET.Length)]);

      return builder.ToString();
    }
  }
}
