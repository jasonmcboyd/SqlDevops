using System.Management.Automation;
using System.Security;

namespace SqlDevOps.PSCmdlets.BasePSCmdlets
{
  public abstract class BaseDbConnectionPSCmdlet : BaseSqlDevOpsPSCmdlet
  {
    #region Parameters

    protected const string PSN_AUTHENTICATE_WITH_CONNECTION_STRING = "AuthenticateWithConnectionString";
    protected const string PSN_AUTHENTICATE_WITH_CREDENTIALS = "AuthenticateWithCredentials";
    protected const string PSN_AUTHENTICATE_WITH_INTEGRATED_SECURITY = "AuthenticateWithIntegratedSecurity";

    [Parameter(
      Mandatory = true,
      ParameterSetName = PSN_AUTHENTICATE_WITH_CONNECTION_STRING,
      Position = 0)]
    public SecureString? ConnectionString { get; set; }

    [Parameter(
      Mandatory = true,
      ParameterSetName = PSN_AUTHENTICATE_WITH_CREDENTIALS,
      Position = 0)]
    [Parameter(ParameterSetName = PSN_AUTHENTICATE_WITH_INTEGRATED_SECURITY)]
    public string? Server { get; set; }

    [Parameter(
      ParameterSetName = PSN_AUTHENTICATE_WITH_CONNECTION_STRING,
      Position = 1)]
    [Parameter(
      Mandatory = true,
      ParameterSetName = PSN_AUTHENTICATE_WITH_CREDENTIALS,
      Position = 1)]
    [Parameter(ParameterSetName = PSN_AUTHENTICATE_WITH_INTEGRATED_SECURITY)]
    public string? Database { get; set; }

    [Parameter(
      Mandatory = true,
      ParameterSetName = PSN_AUTHENTICATE_WITH_CREDENTIALS,
      Position = 2)]
    public PSCredential? Credential { get; set; }

    [Parameter(ParameterSetName = PSN_AUTHENTICATE_WITH_INTEGRATED_SECURITY)]
    public SwitchParameter IntegratedSecurity { get; set; }

    #endregion Parameters
  }
}
