
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UsuariosApi.Models;

namespace UsuariosApi.Services;

public class TokenService
{
    public string GenerateToken(Usuario usuario)
    {
        Claim[] claims = new Claim[]
        {
            new Claim("username", usuario.UserName),
            new Claim("id", usuario.Id),
            new Claim(ClaimTypes.DateOfBirth,
            usuario.DataNascimento.ToString()) //Claim é um objeto que representa uma informação sobre o usuário, ele será passado como parametro para o token
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("9SAFAFWEFWE9FEWFWEFEW54F8WEF489WEF"));

        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken
            (
                expires: DateTime.Now.AddMinutes(10),
                claims: claims,
                signingCredentials: signingCredentials
            );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}