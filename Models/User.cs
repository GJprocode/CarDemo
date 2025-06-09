namespace Backend.Models;

// Defining User entity that matches Users Table in SQL SCHEMA, AngularCarDemo
// provides properties for CRUD ops & authentication.
// Add data annotaion later like required and Bcrypt new new for .NET
public class User
{
    public int Id { get; set; }

    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Username { get; set; }

}
