using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Reflection;
using System.Text;
using WebApiTrainingProject.Data;
using WebApiTrainingProject.Middlewares;
using WebApiTrainingProject.NodeSystem;
using WebApiTrainingProject.NodeSystem.Nodes;
using WebApiTrainingProject.Repositories.Implementations;
using WebApiTrainingProject.Repositories.Interfaces;
using WebApiTrainingProject.Services.Implementations;
using WebApiTrainingProject.Services.Interfaces;
using WebApiTrainingProject.Utils;
using WebApiTrainingProject.Utils.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Log
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// DB
string connectionString = builder.Configuration.GetConnectionString("DockerDBConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<INodeGraphRepository, NodeGraphRepository>();

// Services
builder.Services.AddSingleton<IJwtTokenService, JwtTokenService>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<INodeGraphService, NodeGraphService>();

builder.Services.AddScoped<INodeExecutorService, NodeExecutorService>();
builder.Services.AddSingleton<NodeFactory>();


// JWT
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();


// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Exeptions
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

// AutoMapper
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// Controllers
builder.Services.AddControllers();

// FluentValidation
builder.Services.AddFluentValidationStartup();

var app = builder.Build();
Log.Information("Application starting...");

app.UseMiddleware<ExceptionHandlingMiddleware>();

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
app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


