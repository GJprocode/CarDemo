using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services;

public class CarService
{
    private readonly AppDbContext _context;

    public CarService(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IQueryable<Car> GetQueryableCars()
    {
        return _context.Cars.AsQueryable();
    }

    public async Task<List<Car>> GetAllCars()
    {
        return await _context.Cars.ToListAsync();
    }
}