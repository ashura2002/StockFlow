using Application;
using Domain.Exceptions;
using FluentValidation;
using Infrastructure;
using Infrastructure.Data;
using Serilog;
using WebAPI;
using WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);


// configure Sentry for error monitoring
builder.WebHost.UseSentry(options =>
{
    options.Dsn = builder.Configuration["Sentry:Dsn"];

    // exceptions that are expected and should not create Sentry issues
    var ignoreExceptions = new[]
    {
        typeof(DomainNotFoundException),
        typeof(DomainConflictException),
        typeof(DomainRuleViolationException),
        typeof(DomainUnauthorizedException),
        typeof(ValidationException)
    };

    // filter captured Sentry events before they are sent
    options.SetBeforeSend((sentryEvent, hint) =>
    {
        var exception = sentryEvent.Exception;

        // ignore expected application exceptions to prvent sentry noise
        if (exception is not null && ignoreExceptions.Contains(exception.GetType()))
        {
            return null;
        }

        // send unexpected exceptions to Sentry for investigation
        return sentryEvent;
    });
});

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
app.UseSerilogRequestLogging(); // log all successfull request at elapsed time
app.UseExceptionHandler();
app.UseCors("AllowAll");
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/", () => new
{
    name = "StockFlow",
    status = "Running"
});
app.MapHealthChecks("/health");
app.Run();