using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.Controllers;

// Handles POST /api/auth/login and /api/auth/register with JSON body, returning a JWT token or error.
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel login)
    {
        try
        {
            if (login == null || string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
                return BadRequest("Email and password are required.");

            var result = await _authService.Authenticate(login.Email, login.Password);
            if (result.Error != null) return Unauthorized(result.Error);
            return Ok(new { token = result.Token });
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "Login failed for user {Email}", login?.Email);
            throw; // Re-throw for global error handling or logging in middleware
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        try
        {
            if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
                return BadRequest("Email, username, and password are required.");

            var result = await _authService.RegisterUser(model.Email, model.Username, model.Password);
            if (result.Error != null) return BadRequest(result.Error);
            return Ok(new { token = result.Token });
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "Registration failed for user {Username}", model?.Username);
            throw;
        }
    }
}

public class LoginModel
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class RegisterModel
{
    public required string Email { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}