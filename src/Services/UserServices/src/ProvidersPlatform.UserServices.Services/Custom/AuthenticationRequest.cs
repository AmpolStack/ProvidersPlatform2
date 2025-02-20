namespace ProvidersPlatform.UserServices.Services.Custom;

public class AuthenticationRequest
{
    public required string Token { get; set; }
    public required string RefreshToken { get; set; }
}