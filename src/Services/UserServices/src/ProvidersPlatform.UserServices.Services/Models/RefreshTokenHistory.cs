namespace ProvidersPlatform.UserServices.Services.Models;

public partial class RefreshTokenHistory
{
    public int IdRefreshToken { get; set; }

    public int IdUser { get; set; }

    public string Token { get; set; } = null!;

    public string RefreshToken { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public DateTime ExpirationDate { get; set; }

    public bool? IsActive { get; set; }
}
