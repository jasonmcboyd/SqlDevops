using System.Management.Automation;
using System.Threading;

namespace SqlDevOps.PSCmdlets.BasePSCmdlets
{
  public abstract class BaseSqlDevOpsPSCmdlet : PSCmdlet
  {
    protected string NormalizePath(string path) => SessionState.Path.GetUnresolvedProviderPathFromPSPath(path);

    protected CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();

    // TODO: Confirm this works.
    protected override void StopProcessing()
    {
      CancellationTokenSource.Cancel();
      base.StopProcessing();
    }

  }
}
