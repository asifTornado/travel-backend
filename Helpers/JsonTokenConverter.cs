using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using backEnd.Models;
using backEnd.Helpers.IHelpers;



namespace backEnd.Helpers;
public class JwtTokenConverter:IJwtTokenConverter
{
    private readonly string _secretKey;

    public JwtTokenConverter()
    {
        _secretKey = "secretKeyadfsssssssssssssssssssssssweewfewwwwwwwwwwwwwwwwwwwwwwwwwwwwwweqeqwewqeqweqweqweqweqwe";
    }

    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_secretKey);

        var claims = new List<Claim>
        {
          
            new Claim(ClaimTypes.Name, user.Id.ToString())
            // Add additional claims as needed
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(10), // Token expiration time
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public int? ParseToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_secretKey);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

        try
        {
            var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            var userIdClaim = claimsPrincipal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
            return Convert.ToInt32(userIdClaim.Value);
        }
        catch
        {
            return null; // Invalid token
        }
    }
}



