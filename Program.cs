using Backend.Data;
using Backend.Middleware;
using Backend.Models;
using Backend.Services;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Serilog;

class Program
{
    private static void Main(string[] args)
    {
        try
        {
            Env.Load();

            var builder = WebApplication.CreateBuilder(args);

            try
            {
                // Configure Serilog with file, console, and SQL Server
                builder.Host.UseSerilog((context, services, configuration) => configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .WriteTo.File("logs/carDemo.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7)
                    .WriteTo.MSSqlServer(
                        connectionString: Environment.GetEnvironmentVariable("DEFAULT_CONNECTION"),
                        sinkOptions: new Serilog.Sinks.MSSqlServer.MSSqlServerSinkOptions
                        {
                            TableName = "Serilogs",
                            AutoCreateSqlTable = true
                        },
                        columnOptions: new Serilog.Sinks.MSSqlServer.ColumnOptions()));
            }
            catch (Exception ex)
            {
                Serilog.Log.Fatal(ex, "Serilog configuration failed");
                throw;
            }

            // Access environment variables
            builder.Configuration["ConnectionStrings:DefaultConnection"] = Environment.GetEnvironmentVariable("DEFAULT_CONNECTION") ?? throw new InvalidOperationException("DEFAULT_CONNECTION not set.");
            builder.Configuration["Jwt:Key"] = Environment.GetEnvironmentVariable("JWT_KEY") ?? throw new InvalidOperationException("JWT_KEY not set.");
            builder.Configuration["Jwt:Issuer"] = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? throw new InvalidOperationException("JWT_ISSUER not set.");
            builder.Configuration["Jwt:Audience"] = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? throw new InvalidOperationException("JWT_AUDIENCE not set.");

            // Add CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngular", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            builder.Services.AddControllers();
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));
            builder.Services.AddScoped<CarService>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAngular"); // Enable CORS with the defined policy

            app.UseCustomMiddleware();

            try
            {
                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var seeder = new DataSeeder(services.GetRequiredService<AppDbContext>(), services.GetRequiredService<AuthService>());
                    seeder.SeedData();
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Seeding failed");
                throw;
            }

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
        catch (Exception ex)
        {
            Serilog.Log.Fatal(ex, "Application failed to start");
            throw;
        }
        finally
        {
            Serilog.Log.CloseAndFlush();
        }
    }
}