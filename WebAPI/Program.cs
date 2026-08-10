using Application;
using Infrastructure;
using Infrastructure.Data;
using Serilog;
using WebAPI;
using WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);
// logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(
        "logs/inventory-.log",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddSwaggerDocumentation();

// middleware registration becuase i use Interface IMiddleware
builder.Services.AddTransient<GlobalExceptionHandler>();

var app = builder.Build();

// seeded data
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
    await seeder.SeedAdminUser();
}


// for Swaggger UI
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseMiddleware<GlobalExceptionHandler>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => new
{
    name = "Inventory Management API",
    status = "Running"
});
app.Run();
