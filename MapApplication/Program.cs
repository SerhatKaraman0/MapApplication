using MapApplication.Data;
using MapApplication.Interfaces;
using MapApplication.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register the DbContext with PostgreSQL using the connection string from appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("WebApiDatabase")));  // Ensure this matches the appsettings.json

// Register your services
 // Register DatabaseOperationsService as IDatabaseOperationsService
builder.Services.AddScoped<IPointService, PointService>();  // Register PointService as IPointService
builder.Services.AddScoped<IResponseService, ResponseService>();  // Register ResponseService as IResponseService

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
