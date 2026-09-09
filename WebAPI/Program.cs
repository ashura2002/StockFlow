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
        "logs/StockFlow-.log",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddJwtAuthenticationDI(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddSwaggerDocumentation();

// cors policy
builder.Services.AddCorsPolicy();
// health check
builder.Services.AddHealthChecks();
// for rate limiting 
builder.Services.AddRateLimiting();
// for cachingggg
builder.Services.AddMemoryCache();

// error handling middleware with problem details
builder.Services.AddExceptionHandler<GlobalErrorHandler>();
builder.Services.AddProblemDetails();

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
app.UseExceptionHandler();
app.UseSerilogRequestLogging(); // log all successfull request at elapsed time
app.UseCors("AllowAll");
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/", () => new
{
    name = "Inventory Management API",
    status = "Running"
});
app.MapHealthChecks("/health");
app.Run();