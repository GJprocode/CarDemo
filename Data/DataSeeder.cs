using Backend.Models;
using Backend.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Backend.Data;

public class DataSeeder
{
    private readonly AppDbContext _context;
    private readonly AuthService _authService;

    public DataSeeder(AppDbContext context, AuthService authService)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
    }

    public void SeedData()
    {
        if (!_context.Cars.Any())
        {
            var random = new Random();
            var brands = new[] { "Toyota", "Honda", "Ford", "BMW", "Mercedes" };
            var colours = new[] { "Red", "Blue", "Black", "White", "Silver" };
            var cars = Enumerable.Range(1, 10).Select(i => new Car
            {
                Id = i,
                CarBrand = brands[random.Next(brands.Length)],
                CarColour = colours[random.Next(colours.Length)],
                CarPrice = (decimal)(random.Next(20000, 60000) + random.NextDouble()),
                ModelDate = DateTime.Now.AddYears(random.Next(-10, 0)),
                InStock = random.Next(2) == 0 ? "Yes" : "No"
            }).ToList();

            _context.Cars.AddRange(cars);
            _context.SaveChanges();
        }

        if (!_context.Users.Any())
        {
            var user = new User { Email = "admin@example.com", Username = "admin", Password = "Admin@1234" };
            _authService.HashPassword(user);
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }
}