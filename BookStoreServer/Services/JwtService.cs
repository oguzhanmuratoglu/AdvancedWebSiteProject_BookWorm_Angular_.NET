using BookStoreServer.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStoreServer.Services;

public class JwtService
{
    public string CreateToken(User appUser, bool rememberMe)
    {
        string token = string.Empty;

        List<Claim> claims = new();
        claims.Add(new("UserId", appUser.Id.ToString()));
        claims.Add(new("FirstName", appUser.Name.ToString()));
        claims.Add(new("LastName", appUser.LastName.ToString()));
        claims.Add(new("Email", appUser.Email ?? string.Empty));
        claims.Add(new("UserName", appUser.DisplayName ?? string.Empty));

        DateTime expiresTime = rememberMe ? DateTime.Now.AddMonths(1) : DateTime.Now.AddDays(1);

        JwtSecurityToken jwtSecurityToken = new(
            issuer: "Oguzhan Muratoglu",
            audience: "Book Store Web Project",
            claims: claims,
            notBefore: DateTime.Now,
            expires: expiresTime,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("my secret key my secret key my secret key 1234 ... my secret key my secret key my secret key 1234 ...")), SecurityAlgorithms.HmacSha512));

        JwtSecurityTokenHandler handler = new();
        token = handler.WriteToken(jwtSecurityToken);

        return token;
    }
}
