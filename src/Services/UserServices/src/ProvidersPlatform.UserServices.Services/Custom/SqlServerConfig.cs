using System.Text;

namespace ProvidersPlatform.UserServices.Services.Custom;

public class SqlServerConfig
{
    public string? Server { get; set; }
    public string? Database { get; set; }
    public bool Trusted_Connection { get; set; }
    public bool TrustServerCertificate  { get; set; }

    public string ConnectionString()
    {
        var sb = new StringBuilder();
        sb.Append($"Server={Server};");
        sb.Append($"Database={Database};");
        sb.Append($"Trusted_Connection={Trusted_Connection};");
        sb.Append($"TrustServerCertificate={TrustServerCertificate};");
        return sb.ToString();
    }
}