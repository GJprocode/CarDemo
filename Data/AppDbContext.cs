using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Backend.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Car> Cars { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("DEFAULT_CONNECTION"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>(entity =>
        {
            entity.Property(e => e.CarPrice).HasColumnType("decimal(18,2)");
        });
    }
}