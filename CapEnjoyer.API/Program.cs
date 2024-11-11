using CapEnjoyer.BL.Middleware;
using CapEnjoyer.DAL;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

Env.Load();
var connectionString = $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
                       $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
                       $"Username={Environment.GetEnvironmentVariable("DB_USERNAME")};" +
                       $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};" +
                       $"Database={Environment.GetEnvironmentVariable("DB_NAME")}";

builder.Services.AddControllers();
builder.Services.AddDbContextFactory<CapEnjoyerDbContext>(
    options => options.UseNpgsql(connectionString));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Hint: Token is \"token\" ;)",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<LoggerMiddleware>();
app.UseMiddleware<ErrorLoggingMiddleware>();
app.UseMiddleware<AuthenticationMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
