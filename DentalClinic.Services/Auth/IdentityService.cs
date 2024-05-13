using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using DentalClinic.Services.Options;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DentalClinic.Services.Auth;
public class IdentityService
{
    private readonly JwtSettings _jwtSettings;
    private readonly byte[] _key;
    public IdentityService(IOptions<JwtSettings> jwtOptions)
    {
        _jwtSettings = jwtOptions.Value;
        _key = Encoding.ASCII.GetBytes(_jwtSettings?.SigningKey!);
    }

    private static JwtSecurityTokenHandler TokenHandler => new();

    public SecurityToken CreateSecurityToken(ClaimsIdentity identity)
    {
        var tokenDescriptor = GetTokenDescriptor(identity);

        return TokenHandler.CreateToken(tokenDescriptor);
    }

    public string WriteToken(SecurityToken securityToken)
    {
        return TokenHandler.WriteToken(securityToken);
    }

    private SecurityTokenDescriptor GetTokenDescriptor(ClaimsIdentity claims)
    {
        return new SecurityTokenDescriptor()
        {
            Subject = claims,
            Expires = DateTime.UtcNow.AddDays(Convert.ToDouble(_jwtSettings.DaysLiveTime)),
            Issuer = _jwtSettings.Issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature)
        };
    }
}
