using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;

namespace Backend.Services;

public class AuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthService(AppDbContext context, IConfiguration config)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    public record AuthResult(string? Token = null, string? Error = null);

    public async Task<AuthResult> Authenticate(string? username, string? password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            throw new ArgumentException("Username and password are required.");

        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == username); // Use Email for login
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
                return new AuthResult { Error = "Invalid email or password." };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured.")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer is not configured."),
                audience: _config["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience is not configured."),
                claims: new[] { new Claim(ClaimTypes.Name, user.Email), new Claim(ClaimTypes.Role, "User") },
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new AuthResult { Token = new JwtSecurityTokenHandler().WriteToken(token) };
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "Authentication error for user {Username}", username);
            return new AuthResult { Error = "An unexpected error occurred during authentication." };
        }
    }

    public async Task<AuthResult> RegisterUser(string email, string username, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            throw new ArgumentException("Email, username, and password are required.");

        try
        {
            if (await _context.Users.AnyAsync(u => u.Email == email))
                return new AuthResult { Error = "Email already exists." };
            if (await _context.Users.AnyAsync(u => u.Username == username))
                return new AuthResult { Error = "Username already exists." };

            var user = new User { Email = email, Username = username, Password = password }; // Initialize Password
            HashPassword(user); // Hash the password
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return await Authenticate(email, password); // Return token after registration
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "Registration error for user {Username}", username);
            return new AuthResult { Error = "An unexpected error occurred during registration." };
        }
    }

    public void HashPassword(User user)
    {
        try
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password); // Hash the initial password
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "Password hashing failed for user {Username}", user?.Username);
            throw;
        }
    }
}