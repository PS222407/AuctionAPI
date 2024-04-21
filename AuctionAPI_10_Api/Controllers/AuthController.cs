using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AuctionAPI_10_Api.RequestModels;
using AuctionAPI_10_Api.ViewModels;
using AuctionAPI_20_BusinessLogic.Models;
using AuctionAPI_30_DataAccess.Data;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AuctionAPI_10_Api.Controllers;

[Route("api")]
[ApiController]
public class AuthController(
    DataContext context,
    IConfiguration config,
    IValidator<UserRequest> userValidator,
    IValidator<RefreshTokenRequest> refreshTokenValidator)
    : ControllerBase
{
    [HttpPost("Register")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public ActionResult Register([FromBody] UserRequest userRequest)
    {
        ValidationResult result = userValidator.Validate(userRequest);
        if (!result.IsValid)
        {
            return BadRequest(new { result.Errors });
        }
        
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(userRequest.Password);
        
        context.Users.Add(new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = userRequest.Email,
            Password = passwordHash,
        });
        context.SaveChanges();
        
        return NoContent();
    }
    
    [HttpPost("Login")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public ActionResult<RefreshTokenViewModel> Login([FromBody] UserRequest userRequest)
    {
        ValidationResult result = userValidator.Validate(userRequest);
        if (!result.IsValid)
        {
            return BadRequest(new { result.Errors });
        }
        
        User? user = context.Users.Include(u => u.Roles).Include(user => user.RefreshTokens)
            .FirstOrDefault(u => u.Email == userRequest.Email);
        if (user == null)
        {
            return Unauthorized();
        }
        
        if (!BCrypt.Net.BCrypt.Verify(userRequest.Password, user.Password))
        {
            return Unauthorized();
        }
        
        RefreshToken refreshToken = GenerateRefreshToken();
        context.RefreshTokens.RemoveRange(user.RefreshTokens);
        context.SaveChanges();
        
        user.RefreshTokens.Add(refreshToken);
        context.SaveChanges();
        
        return Ok(new
        {
            AccessToken = GenerateJwtToken(user),
            RefreshToken = refreshToken.Token,
            TokenType = "Bearer",
            ExpiresIn = config.GetValue<int>("Jwt:ExpiresIn"),
        });
    }
    
    [HttpPost("Refresh")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public ActionResult<AccessTokenViewModel> Refresh([FromBody] RefreshTokenRequest refreshTokenRequest)
    {
        ValidationResult result = refreshTokenValidator.Validate(refreshTokenRequest);
        if (!result.IsValid)
        {
            return BadRequest(new { result.Errors });
        }
        
        User? user = context.Users
            .Include(u => u.Roles)
            .Include(u => u.RefreshTokens)
            .FirstOrDefault(u => u.RefreshTokens.Any(rt => rt.Token == refreshTokenRequest.RefreshToken));
        if (user == null)
        {
            return Unauthorized();
        }
        
        return Ok(new
        {
            AccessToken = GenerateJwtToken(user),
            TokenType = "Bearer",
            ExpiresIn = config.GetValue<int>("Jwt:ExpiresIn"),
        });
    }
    
    private string GenerateJwtToken(User user)
    {
        List<Claim> claims =
        [
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
        ];
        user.Roles.ForEach(r => claims.Add(new Claim(ClaimTypes.Role, r.Name)));
        
        SymmetricSecurityKey symmetricSecurityKey = new(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        SigningCredentials credentials = new(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        
        JwtSecurityToken jwtSecurityToken = new(
            audience: config["Jwt:Audience"],
            issuer: config["Jwt:Issuer"],
            claims: claims,
            expires: DateTime.UtcNow.AddSeconds(config.GetValue<int>("Jwt:ExpiresIn")),
            signingCredentials: credentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }
    
    private static RefreshToken GenerateRefreshToken()
    {
        return new RefreshToken
        {
            Id = Guid.NewGuid().ToString(),
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
        };
    }
}