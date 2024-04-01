using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API_Service_Auth.Configs;
using API_Service_Auth.Models.Entities;
using API_Service_Auth.Models.Requests;
using API_Service_Auth.Models.Responses;
using catat.utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API_Service_Auth.Services;

public class AuthService
{

    private readonly IConfiguration _config;

    public AuthService(IConfiguration config)
    {
        _config = config;
    }

    public async Task<Response<LoginResponse>> Login(LoginRequest request)
    {
        if (string.IsNullOrEmpty(request.UserName))
        {
            return new Response<LoginResponse>
            {
                Message = "Username must not be empty",
                Success = false
            };
        }

        if (string.IsNullOrEmpty(request.Password))
        {
            return new Response<LoginResponse>
            {
                Message = "Password must not be empty",
                Success = false
            };
        }

        if (string.IsNullOrEmpty(request.TenantCode))
        {
            return new Response<LoginResponse>
            {
                Message = "Tenant Code must not be empty",
                Success = false
            };
        }

        var encrypt = new Encrypt();
        var hashedPassword = encrypt.HashPassword(request.Password);

        using (var context = new DataContext())
        {
            var tenant = await context.Tenants.FirstOrDefaultAsync(t => t.TenantCode == request.TenantCode);
            if (tenant == null)
            {
                return new Response<LoginResponse>
                {
                    Message = "Invalid Tenant Code",
                    Success = false
                };
            }

            var user = await context.UserMsts.FirstOrDefaultAsync(u => u.UserName == request.UserName && u.Password == hashedPassword);
            if (user == null)
            {
                return new Response<LoginResponse>
                {
                    Message = "Invalid username or password",
                    Success = false
                };
            }

            var key = _config["Jwt:Key"];
            var expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:ValidityInMinutes"]));
            var refreshTokenExpires = DateTime.UtcNow.AddDays(Convert.ToDouble(_config["Jwt:RefreshTokenValidityInDays"]));

            return new Response<LoginResponse>
            {
                Message = "Login successful",
                Data = new LoginResponse
                {
                    Token = GenerateJwtToken(user, key, expires),
                    RefreshToken = GenerateRefreshToken(user, key, refreshTokenExpires)
                },
                Success = true
            };
        }
    }

    private string GenerateJwtToken(UserMst user, string key, DateTime expires)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.ASCII.GetBytes(key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.UserName)
            }),
            Expires = expires,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private string GenerateRefreshToken(UserMst user, string key, DateTime expires)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.ASCII.GetBytes(key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("RefreshToken", "true")
            }),
            Expires = expires,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
