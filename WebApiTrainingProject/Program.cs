using Microsoft.EntityFrameworkCore;
using Serilog;
using WebApiTrainingProject.Data;

var builder = WebApplication.CreateBuilder(args);

// Log
// Log.Logger = new LoggerConfiguration().ReadFrom
//builder.Host.UseSerilog();

// DB
string connectionString = builder.Configuration.GetConnectionString("DockerDBConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Controllers
builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

// Swagger
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();