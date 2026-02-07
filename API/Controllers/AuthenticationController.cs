using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.DTOs;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers;

[AllowAnonymous]
public class AuthenticationController(UserManager<AppUser> userManager, IConfiguration configuration) : BaseAPIController
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO loginInfo)
    {
        AppUser? user = await userManager.FindByEmailAsync(loginInfo.Email);
        if (user != null)
        {
            if (await userManager.CheckPasswordAsync(user, loginInfo.Password))
            {
                List<Claim> claims = new()
                {
                    new(ClaimTypes.NameIdentifier, user.Id),
                    new(ClaimTypes.Name, user.FirstName ?? ""),
                    new(ClaimTypes.Email, user.Email ?? "")
                };
                string? accessToken = GenerateJWT(claims);
                return Ok(new { accessToken });
            }
        }
        return Unauthorized();
    }

    private string? GenerateJWT(List<Claim> claims)
    {
        string secretKey = configuration["JWT:SecretKey"] ?? "";
        string audience = configuration["JWT:Audience"] ?? "";
        string issuer = configuration["JWT:Issuer"] ?? "";

        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(secretKey));
        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);
        return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: credentials
        ));
    }
}
