using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProvidersPlatform.Shared.Models;
using ProvidersPlatform.UserServices.Services.Custom;
using ProvidersPlatform.UserServices.Services.Definitions;
using ProvidersPlatform.UserServices.Services.Models;

namespace ProvidersPlatform.UserServices.Services.Implementations;

public class JwtAuthentication : IJwtAuthentication
{
    private readonly JwtDbContext _context;
    private readonly JwtConfig _jwtConfig;

    public JwtAuthentication(JwtDbContext context, IOptions<JwtConfig> jwtConfig)
    {
        _context = context;
        _jwtConfig = jwtConfig.Value;
    }


    public async Task<bool> SessionAlreadyExist(AuthenticationRequest request, int id, CancellationToken cancellationToken)
    {
        var sessionFind = await _context.RefreshTokenHistories.FirstOrDefaultAsync(x =>
                x.IdUser == id &&
                x.Token == request.Token &&
                x.RefreshToken == request.RefreshToken
            , cancellationToken);

        if (sessionFind == null)
        {
            return false;
        }
        
        return sessionFind!.ExpirationDate < DateTime.UtcNow;
    }

    public async Task<AuthenticationResponse> Authenticate(User user, CancellationToken cancellationToken)
    {
        if (user.Email.Equals(string.Empty) || user.Password.Equals(string.Empty))
            return new AuthenticationResponse()
            {
                Success = false,
                Message = "Email or password are empty"
            };

        var claims = new Claim[]
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("temp", "1|2"),
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
        };

        var token = GenerateJwtToken(claims, _jwtConfig.Key!, _jwtConfig.LifeTimeInMinutes);
        var refreshToken = GenerateRefreshToken();

        return await SaveRefreshToken(user.UserId, token, refreshToken, cancellationToken);
    }

    private async Task<AuthenticationResponse> SaveRefreshToken(int id, string token, string refreshToken,
        CancellationToken cancellationToken)
    {
        if (token.Equals(string.Empty) || refreshToken.Equals(string.Empty)) 
            return new AuthenticationResponse()
            {
                Success = false,
                Message = "The tokens are impossible to create in this moment, please try again later."
            };
        
        await _context.RefreshTokenHistories.AddAsync(new RefreshTokenHistory()
        {
            IdUser = id,
            Token = token,
            RefreshToken = refreshToken,
            CreationDate = DateTime.UtcNow
        }, cancellationToken);

        return new AuthenticationResponse()
        {
            Success = true,
            Message = "success authentication",
            Token = token,
            RefreshToken = refreshToken
        };
    }
    private static string GenerateJwtToken(Claim[] claims, string key, double lifeTimeInMinutes)
    {
        
        var identity = new ClaimsIdentity(claims);
        var keyBytes = Encoding.UTF8.GetBytes(key);
        var credentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), 
            SecurityAlgorithms.HmacSha256Signature);
        
        var descriptor = new SecurityTokenDescriptor()
        {
            Expires = DateTime.UtcNow.AddMinutes(lifeTimeInMinutes),
            Subject = identity,
            SigningCredentials = credentials
            
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(descriptor);
        return tokenHandler.WriteToken(securityToken);
    }
    
    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }
        return Convert.ToBase64String(randomNumber);
    }
}