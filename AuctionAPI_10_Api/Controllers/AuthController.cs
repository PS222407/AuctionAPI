using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AuctionAPI_10_Api.RequestModels;
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
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;

    private readonly DataContext _context;

    private readonly IValidator<RefreshTokenRequest> _refreshTokenValidator;

    private readonly IValidator<UserRequest> _userValidator;

    public AuthController(
        DataContext context,
        IConfiguration config,
        IValidator<UserRequest> userValidator,
        IValidator<RefreshTokenRequest> refreshTokenValidator)
    {
        _context = context;
        _config = config;
        _userValidator = userValidator;
        _refreshTokenValidator = refreshTokenValidator;
    }

    [HttpPost("Register")]
    public ActionResult Register([FromBody] UserRequest userRequest)
    {
        ValidationResult result = _userValidator.Validate(userRequest);
        if (!result.IsValid)
        {
            return BadRequest(new { result.Errors });
        }

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(userRequest.Password);

        _context.Users.Add(new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = userRequest.Email,
            Password = passwordHash,
        });
        _context.SaveChanges();

        return NoContent();
    }

    [HttpPost("Login")]
    public ActionResult<User> Login([FromBody] UserRequest userRequest)
    {
        ValidationResult result = _userValidator.Validate(userRequest);
        if (!result.IsValid)
        {
            return BadRequest(new { result.Errors });
        }

        User? user = _context.Users.Include(u => u.Roles).FirstOrDefault(u => u.Email == userRequest.Email);
        if (user == null)
        {
            return Unauthorized();
        }

        if (!BCrypt.Net.BCrypt.Verify(userRequest.Password, user.Password))
        {
            return Unauthorized();
        }

        RefreshToken refreshToken = GenerateRefreshToken();
        user.RefreshTokens.Add(refreshToken);
        _context.SaveChanges();

        return Ok(new
        {
            AccessToken = GenerateJwtToken(user),
            RefreshToken = refreshToken.Token,
            TokenType = "Bearer",
            ExpiresIn = _config.GetValue<int>("Jwt:ExpiresIn"),
        });
    }

    [HttpPost("Refresh")]
    public ActionResult Refresh([FromBody] RefreshTokenRequest refreshTokenRequest)
    {
        ValidationResult result = _refreshTokenValidator.Validate(refreshTokenRequest);
        if (!result.IsValid)
        {
            return BadRequest(new { result.Errors });
        }

        User? user = _context.Users
            .Include(u => u.Roles)
            .Include(u => u.RefreshTokens)
            .FirstOrDefault(u => u.RefreshTokens.Any(rt => rt.Token == refreshTokenRequest.RefreshToken));
        if (user == null)
        {
            return Unauthorized();
        }

        RefreshToken refreshToken = GenerateRefreshToken();
        RefreshToken tokenToDelete = user.RefreshTokens.First(rt => rt.Token == refreshTokenRequest.RefreshToken);
        _context.RefreshTokens.Remove(tokenToDelete);
        _context.SaveChanges();

        user.RefreshTokens.Add(refreshToken);
        _context.SaveChanges();

        return Ok(new
        {
            AccessToken = GenerateJwtToken(user),
            RefreshToken = refreshToken.Token,
            TokenType = "Bearer",
            ExpiresIn = _config.GetValue<int>("Jwt:ExpiresIn"),
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

        SymmetricSecurityKey symmetricSecurityKey = new(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        SigningCredentials credentials = new(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken jwtSecurityToken = new(
            audience: _config["Jwt:Audience"],
            issuer: _config["Jwt:Issuer"],
            claims: claims,
            expires: DateTime.UtcNow.AddSeconds(_config.GetValue<int>("Jwt:ExpiresIn")),
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