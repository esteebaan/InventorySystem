using InventorySystem.API.DTOs;
using InventorySystem.API.Security;
using InventorySystem.DataAccess;
using InventorySystem.Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InventorySystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly JwtSettings _jwt;

    public AuthController(AppDbContext context, IOptions<JwtSettings> jwtOptions)
    {
        _context = context;
        _jwt = jwtOptions.Value;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        // Simulación de búsqueda de usuario con contraseña hasheada
        var user = _context.Users
        .FirstOrDefault(u => u.Email == dto.Email && u.PasswordHash == Convert.ToBase64String(Encoding.UTF8.GetBytes(dto.Password)));

        if (user == null)
            return Unauthorized(new { message = "Invalid credentials" });

        var employee = _context.Employees.FirstOrDefault(e => e.UserId == user.UserId);
        var roleName = employee != null
            ? _context.Roles.FirstOrDefault(r => r.RoleId == employee.RoleId)?.Name ?? "Unknown"
            : "Operator"; // 👈 default for non-admins

        var token = GenerateToken(user.Email, roleName);

        return Ok(new
        {
            token,
            employeeId = employee?.EmployeeId ?? Guid.Empty, // 👈 nunca null
            role = roleName
        });
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserDto dto)
    {
        var emailExists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
        if (emailExists)
            return BadRequest(new { message = "Email already exists." });

        var user = new User
        {
            UserId = Guid.NewGuid(),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PasswordHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(dto.Password)),
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);

        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == dto.Role);
        if (role == null)
            return BadRequest(new { message = "Role not found." });

        var employee = new Employee
        {
            EmployeeId = Guid.NewGuid(),
            UserId = user.UserId,
            RoleId = role.RoleId
        };

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        var token = GenerateToken(user.Email, role.Name);

        return Ok(new
        {
            token,
            employeeId = employee.EmployeeId,
            role = role.Name
        });
    }


    private string GenerateToken(string email, string role)
    {
        var claims = new[]
        {
            new Claim("email", email),
            new Claim("role", role) // necesario para [Authorize(Roles = "...")]
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    }
public class LoginDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
