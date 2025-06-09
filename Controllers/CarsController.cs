using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CarsController : ControllerBase
{
    private readonly CarService _carService;

    public CarsController(CarService carService)
    {
        _carService = carService ?? throw new ArgumentNullException(nameof(carService));
    }

    [HttpGet]
    public async Task<IActionResult> GetCars()
    {
        try
        {
            var cars = await _carService.GetAllCars();
            return Ok(cars);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "Error fetching cars");
            return StatusCode(500, "Error fetching cars");
        }
    }
}