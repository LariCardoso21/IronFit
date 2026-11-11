using Microsoft.EntityFrameworkCore;
using IronFit.Data;

var builder = WebApplication.CreateBuilder(args);

// Configura o banco MySQL via Entity Framework
string connectionString = "Server=localhost;Database=saep_db;Uid=root;Pwd=;";
builder.Services.AddDbContext<BancoContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
