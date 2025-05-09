using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Serilog;
using StoreApi.Extensions;
using StoreApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddLogging();

builder.Services.AddDbContext<StoreContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresqlConnection"))
);
builder.Services.AddEntityRepositories();
builder.Services.AddEntityServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });
builder.Services.ConfigureCors();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();

app.ConfigureExceptionHandler(logger);

if (app.Environment.IsProduction())
{
    app.UseHsts();
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();