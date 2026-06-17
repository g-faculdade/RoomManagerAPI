using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace RoomManagerAPI.Services;

public class TokenService
{
    public string GerarToken()
    {
        var chave = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("minha-chave-super-secreta-123456")
        );

        var credenciais = new SigningCredentials(
            chave,
            SecurityAlgorithms.HmacSha256
        );

        var claims = new[]
        {
            new Claim(ClaimTypes.Email, "admin@email.com")
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credenciais
        );

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}