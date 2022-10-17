namespace SqlDevOps.Utilities
{
  internal struct TreeNode<T>
  {
    public TreeNode(int depth, T value)
    {
      Depth = depth;
      Value = value;
    }

    public int Depth { get; }
    public T Value { get; }
  }
}
