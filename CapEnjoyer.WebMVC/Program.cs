using CapEnjoyer.BL.Mappers;
using CapEnjoyer.API.Helpers;
using CapEnjoyer.DAL;
using CapEnjoyer.DAL.Entities;
using DotNetEnv;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("LocalPostgres");
var postgresOptionValidator = new PostgresOptionValidator { ConnectionString = connectionString };

builder.Services.AddDbContextFactory<CapEnjoyerDbContext>(
    options => options.UseNpgsql(postgresOptionValidator.ConnectionString));


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddIdentity<LocalIdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<CapEnjoyerDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 0;
    options.Password.RequiredUniqueChars = 0;
});
builder.Services.AddMapster();
TypeAdapterConfig.GlobalSettings.ConfigureAlbumMapping();

builder.Services.ConfigureApplicationCookie(options => options.LoginPath = "/Login");
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.ValidateConnection();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

app.Run();
