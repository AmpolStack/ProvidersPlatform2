using System.Text;
using Microsoft.Extensions.Primitives;

namespace ProvidersPlatform.Shared.Setup.Configs;

public class MariaDbConfig
{
    public string? Server { get; set; }
    public string? Database { get; set; }
    public string? Uid { get; set; }
    public string? Pwd { get; set; }
    public string? ServerVersion { get; set; }
    public string GetConnectionString()
    {
        var sb = new StringBuilder();
        sb.Append($"Server={Server};");
        sb.Append($"Database={Database};");
        sb.Append($"Uid={Uid};");
        sb.Append($"Pwd={Pwd};");
        return sb.ToString();
    }
}