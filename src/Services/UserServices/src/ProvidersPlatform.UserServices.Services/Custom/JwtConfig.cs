namespace ProvidersPlatform.UserServices.Services.Custom;

public class JwtConfig
{
    public string? Key { get; set; }
    public double LifeTimeInMinutes { get; set; }
}