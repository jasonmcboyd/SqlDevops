using Microsoft.SqlServer.Dac.Model;

using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace SqlDevOps.Utilities
{
  internal static class TSqlObjectUtilities
  {
    public static string ToCliString(this TSqlObject sqlObject)
    {
      switch (sqlObject.ObjectType.Name)
      {
        case nameof(RoleMembership):
          return $"Role Membership: {sqlObject.GetSourceInformation().SourceName}";
        default:
         return $"{sqlObject.ObjectType.Name}: {sqlObject.Name}";
      }
    }

    public static IEnumerable<TreeNode<TSqlObject>> GetReferencingPreOrderTraversal(this TSqlObject sqlObject)
    {
      // Create a stack to keep track of the branches being traversed.
      var enumerators = new Stack<IEnumerator<TSqlObject>>();
      enumerators.Push(Enumerable.Repeat(sqlObject, 1).GetEnumerator());

      // Track what has been seen already.
      var seen = new HashSet<TSqlObject>();

      // Loop as long as we have an enumerator on the stack. When we have popped the last
      // one off the stack the traversal is complete.
      while (enumerators.Count > 0)
      {
        if (enumerators.Peek().MoveNext())
        {
          sqlObject = enumerators.Peek().Current;

          if (seen.Contains(sqlObject))
            continue;

          seen.Add(sqlObject);

          // Yield the current node then push it and its children onto the 
          // stack.
          yield return new TreeNode<TSqlObject>(enumerators.Count - 1, sqlObject);
          enumerators.Push(sqlObject.GetReferencing().GetEnumerator());
        }
        else
        {
          // Pop the enumerator and dispose of it.
          enumerators.Pop().Dispose();
        }
      }
    }

    public static IEnumerable<TreeNode<TSqlObject>> GetReferencingPostOrderTraversal(this TSqlObject sqlObject)
    {
      // Create a stack to keep track of the branches being traversed.
      Stack<IEnumerator<TSqlObject>> enumerators = new Stack<IEnumerator<TSqlObject>>();
      enumerators.Push(Enumerable.Repeat(sqlObject, 1).GetEnumerator());

      // Track what has been seen already.
      var seen = new HashSet<TSqlObject>();

      // Loop as long as we have an enumerator on the stack.  When we have popped the last
      // one off the stack the traversal is complete.
      while (enumerators.Count > 0)
      {
        if (enumerators.Peek().MoveNext())
        {
          sqlObject = enumerators.Peek().Current;

          if (seen.Contains(sqlObject))
            continue;

          seen.Add(sqlObject);

          // Push the current node's children onto the stack.
          enumerators.Push(sqlObject.GetReferencing().GetEnumerator());
        }
        else
        {
          // Pop the enumerator and dispose of it.
          enumerators.Pop().Dispose();

          // Yield the 'Current' value of the enumerator on the top of the stack.
          if (enumerators.Count > 0)
            yield return new TreeNode<TSqlObject>(enumerators.Count, enumerators.Peek().Current);
        }
      }
    }
  }
}
