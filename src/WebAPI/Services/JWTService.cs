using Application.Dto;
using Domain.Dto;
using Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPI.Services;

public class JWTService : IJWTService
{
    private readonly IConfiguration _config;

    public JWTService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(UserLogin user)
    {
        Guid guid = Guid.NewGuid();
        string role = "Admin";

        string secretKey = _config["Jwt:SecretKey"] ?? throw new InvalidDataException("JWT SecretKey");
        string issuer = _config["Jwt:Issuer"] ?? throw new InvalidDataException("JWT Issuer");
        string audience = _config["Jwt:Audience"] ?? throw new InvalidDataException("JWT audience");

        var key = Encoding.ASCII.GetBytes(secretKey);

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {

            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, guid.ToString()),
                new Claim(ClaimTypes.Role, role)
            }),
            Issuer = issuer,
            Audience = audience,
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public Guid ValidateJwtToken(string? token)
    {
        if (token == null)
            throw new UnauthorizedAccessException();

        var tokenHandler = new JwtSecurityTokenHandler();

        string secretKey = _config["Jwt:SecretKey"] ?? throw new InvalidDataException("JWT SecretKey");
        var key = Encoding.ASCII.GetBytes(secretKey);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

            return userId;
        }
        catch
        {
            throw new UnauthorizedAccessException();
        }
    }
}
