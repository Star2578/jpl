using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JapaneseStudyApi.Data;
using JapaneseStudyApi.Model;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JapaneseStudyContext _context;
    private readonly IConfiguration _configuration;
    private readonly SymmetricSecurityKey _jwtSecretKey;

    public AuthController(JapaneseStudyContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
        _jwtSecretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET")));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(User user)
    {
        // TODO : Create table for role validation instead
        // Check if the role is valid
        if (user.Role != "Admin" && user.Role != "User")
        {
            return BadRequest(new { message = "Invalid role" });
        }

        // Check if the username is unique
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
        if (existingUser != null)
        {
            return Conflict(new { message = "Username already exists" });
        }

        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Ok(new { message = "User registered successfully" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(User user)
    {
        var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Username == user.Username);
        if (existingUser == null || !BCrypt.Net.BCrypt.Verify(user.Password, existingUser.Password))
        {
            return Unauthorized(new { message = "Invalid username or password" });
        }
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                    new Claim(ClaimTypes.NameIdentifier, existingUser.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddDays(7), // Token valid for 7 days
            SigningCredentials = new SigningCredentials(_jwtSecretKey, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        // Log jwtSecret and tokenString for verification
        Console.WriteLine($"JWT Secret: {Environment.GetEnvironmentVariable("JWT_SECRET")}");
        Console.WriteLine($"Generated Token: {tokenString}");

        return Ok(new { Id = existingUser.Id, Token = tokenString });
    }
}