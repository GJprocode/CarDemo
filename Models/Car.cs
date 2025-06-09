namespace Backend.Models;

// Defining Car entity that matches Car table in schema
// add validation attributes and seeding data.
public class Car
{
    public int Id { get; set; }
    public required string CarBrand { get; set; }
    public required string CarColour { get; set; }
    public required decimal CarPrice { get; set; }
    public required DateTime ModelDate { get; set; }
    public required string InStock { get; set; }
}