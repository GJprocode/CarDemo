using Backend.Data;
using Backend.Models;
using Backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Xunit;

namespace CarDemo.Tests;

public class AuthServiceTests : IDisposable
{
    private readonly AuthService _authService;
    private readonly AppDbContext _context;

    public AuthServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB per test
            .Options;
        _context = new AppDbContext(options);
        var config = new ConfigurationBuilder().AddInMemoryCollection(new[]
        {
            new KeyValuePair<string, string?>("Jwt:Key", "testkey12345678901234567890123456789012"),
            new KeyValuePair<string, string?>("Jwt:Issuer", "testissuer"),
            new KeyValuePair<string, string?>("Jwt:Audience", "testaudience")
        }).Build();
        _authService = new AuthService(_context, config);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    private void SeedDatabase()
    {
        var user = new User { Username = "testuser", Password = "testpass", Email = "test@example.com" };
        _authService.HashPassword(user);
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    [Fact]
    public async Task Authenticate_ValidCredentials_ReturnsToken()
    {
        SeedDatabase();
        var result = await _authService.Authenticate("testuser", "testpass");
        Assert.NotNull(result);
        Assert.Null(result.Error);
        Assert.NotNull(result.Token);
        Assert.False(string.IsNullOrEmpty(result.Token));
    }

    [Fact]
    public async Task Authenticate_InvalidPassword_ReturnsError()
    {
        SeedDatabase();
        var result = await _authService.Authenticate("testuser", "wrongpass");
        Assert.NotNull(result);
        Assert.NotNull(result.Error);
        Assert.Equal("Invalid username or password.", result.Error);
        Assert.Null(result.Token);
    }

    [Fact]
    public async Task Authenticate_NullInput_ThrowsArgumentException()
    {
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _authService.Authenticate(null, null));
        Assert.Contains("Username and password are required", exception.Message);
    }
}