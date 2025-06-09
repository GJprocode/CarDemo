using Backend.Data;
using Backend.Models;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Backend.Helpers;



namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CarsController : ControllerBase
{
    private readonly AppDbContext _context;

    public CarsController(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    //  DevExtreme Grid: Filtering, Sorting, Grouping, Paging
    [HttpGet("grid")]
    public IActionResult GetCarsGrid()
    {
        var loadOptions = new DevExtreme.AspNet.Data.DataSourceLoadOptionsBase();

        // Manually parse query parameters
        foreach (var kvp in Request.Query)
        {
            loadOptions.SetOption(kvp.Key, kvp.Value.ToString());
        }

        var result = DataSourceLoader.Load(_context.Cars.AsNoTracking(), loadOptions);
        return new JsonResult(result); // Important: don't wrap in Ok()
    }





    //  Insert Car
    [HttpPost]
    public IActionResult InsertCar([FromBody] IDictionary<string, object> values)
    {
        var car = JsonConvert.DeserializeObject<Car>(JsonConvert.SerializeObject(values));
        if (car == null)
            return BadRequest("Invalid car data.");

        _context.Cars.Add(car);
        _context.SaveChanges();

        return Ok(car);
    }

    //  Update Car by ID
    [HttpPut]
    public IActionResult UpdateCar(int key, [FromBody] IDictionary<string, object> values)
    {
        var car = _context.Cars.FirstOrDefault(c => c.Id == key);
        if (car == null)
            return NotFound();

        JsonConvert.PopulateObject(JsonConvert.SerializeObject(values), car);
        _context.SaveChanges();

        return Ok(car);
    }

    // Delete Car by ID
    [HttpDelete]
    public IActionResult DeleteCar(int key)
    {
        var car = _context.Cars.FirstOrDefault(c => c.Id == key);
        if (car == null)
            return NotFound();

        _context.Cars.Remove(car);
        _context.SaveChanges();

        return Ok();
    }
}
