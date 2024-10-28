using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CoinDesk.Model.Config;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CoinDesk.Utility;

public class JwtTokenGenerator
{
    private readonly IOptions<JwtSettings> _jwtSettings;

    public JwtTokenGenerator(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings;
    }
    public string GenerateJwtToken(string account)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Value.SignKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, account.ToString()),
            new Claim(ClaimTypes.Name, account)
        };
        var token = new JwtSecurityToken(
            issuer: this._jwtSettings.Value.Issuer,
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}