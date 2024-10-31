using Microsoft.Data.SqlClient;
using System.Management.Automation;
using System.Security;
using SqlDevOps.Extensions;

namespace SqlDevOps.PSCmdlets
{
  [Cmdlet(VerbsCommon.New, PSCmdletNouns.SecureConnectionString)]
  [OutputType(typeof(SecureString))]
  public class NewSecureConnectionStringPSCmdlet : PSCmdlet
  {
    protected const string PSN_AUTHENTICATE_WITH_CREDENTIALS = "AuthenticateWithCredentials";
    protected const string PSN_AUTHENTICATE_WITH_INTEGRATED_SECURITY = "AuthenticateWithIntegratedSecurity";

    #region Parameters

    [Parameter(
      Mandatory = true,
      Position = 0)]
    public string? Server { get; set; }

    [Parameter(Position = 1)]
    public string? Database { get; set; }

    [Parameter(
      Mandatory = true,
      ParameterSetName = PSN_AUTHENTICATE_WITH_CREDENTIALS,
      Position = 2)]
    public PSCredential? Credential { get; set; }

    [Parameter(
      Mandatory = true,
      ParameterSetName = PSN_AUTHENTICATE_WITH_INTEGRATED_SECURITY)]
    public SwitchParameter IntegratedSecurity { get; set; }

    [Parameter(
      Mandatory = true)]
    public SwitchParameter TrustServerCertificate { get; set; }

    #endregion Parameters

    protected override void ProcessRecord()
    {
      var builder = new SqlConnectionStringBuilder()
      {
        DataSource = Server!,
      };

      if (!string.IsNullOrEmpty(Database))
      {
        builder.InitialCatalog = Database;
      }

      if (TrustServerCertificate.IsPresent)
      {
        builder.TrustServerCertificate = TrustServerCertificate.ToBool();
      }

      switch (ParameterSetName)
      {
        case PSN_AUTHENTICATE_WITH_CREDENTIALS:
          if (Credential is null) throw new PSArgumentNullException(nameof(Credential));

          builder.Add("User Id", Credential.UserName);
          builder.Add("Password", Credential.GetNetworkCredential().Password);
          break;
        case PSN_AUTHENTICATE_WITH_INTEGRATED_SECURITY:
          builder.Add("Integrated Security", "yes");
          break;
        default:
          // TODO: Evaluate user experience
          throw new PSNotImplementedException();
      }

      WriteObject(builder.ToString().ToSecureString());
    }
  }
}
