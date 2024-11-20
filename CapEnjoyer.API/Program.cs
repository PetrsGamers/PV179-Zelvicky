using CapEnjoyer.API.Helpers;
using CapEnjoyer.API.Middleware;
using CapEnjoyer.BL.Interfaces;
using CapEnjoyer.BL.Middleware;
using CapEnjoyer.BL.Services;
using CapEnjoyer.BL.Services.Interfaces;
using CapEnjoyer.DAL;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

Helpers.CheckIfEnvironmentVariablesAreSet();
var connectionString = Helpers.GetConnectionString();

builder.Services.AddControllers(options => options.RespectBrowserAcceptHeader = true).AddXmlSerializerFormatters();

builder.Services.AddDbContextFactory<CapEnjoyerDbContext>(
    options => options.UseNpgsql(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBottleService, BottleService>();
builder.Services.AddScoped<IAlbumService, AlbumService>();
builder.Services.AddScoped<ICapService, CapService>();
builder.Services.AddScoped<IColorService, ColorService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IMiddlewareLoggingService, MiddlewareLoggingService>();
builder.Services.AddScoped<ILeaderboardService, LeaderboardService>();
builder.Services.AddScoped<IEditRequestService, EditRequestService>();
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

// in case the database is not reachable or not created, throw an error
Helpers.CheckIfDatabaseServerReachableAndCreated(app);

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<LoggerMiddleware>();
app.UseMiddleware<ErrorLoggingMiddleware>();
app.UseMiddleware<AuthenticationMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseStaticFiles();
app.Run();
