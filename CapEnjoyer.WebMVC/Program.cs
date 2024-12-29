using Cap.Enjoyer.WebMVC.Mappers;
using CapEnjoyer.API.Helpers;
using CapEnjoyer.BL.Mappers;
using CapEnjoyer.BL.Services;
using CapEnjoyer.BL.Services.Interfaces;
using CapEnjoyer.DAL;
using CapEnjoyer.DAL.Entities;
using CapEnjoyer.Middleware;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("LocalPostgres");
var postgresOptionValidator = new PostgresOptionValidator { ConnectionString = connectionString };

builder.Services.AddDbContextFactory<CapEnjoyerDbContext>(
    options => options.UseNpgsql(postgresOptionValidator.ConnectionString));

builder.Services.AddMemoryCache();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBottleService, BottleService>();
builder.Services.AddScoped<IAlbumService, AlbumService>();
builder.Services.AddScoped<ICapService, CapService>();
builder.Services.AddScoped<IColorService, ColorService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IMiddlewareLoggingService, MiddlewareLoggingService>();
builder.Services.AddScoped<ILeaderboardService, LeaderboardService>();
builder.Services.AddScoped<IEditRequestService, EditRequestService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProducerService, ProducerService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<ICouponService, CouponService>();

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
TypeAdapterConfig.GlobalSettings.ConfigureAlbumMVCMapping();
TypeAdapterConfig.GlobalSettings.EnableImmutableMapping();
TypeAdapterConfig.GlobalSettings.ConfigureEditRequestMapping();
TypeAdapterConfig.GlobalSettings.ConfigureCapMapping();
TypeAdapterConfig.GlobalSettings.ConfigureBottleMapping();
TypeAdapterConfig.GlobalSettings.ConfigureProducerMapping();

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
app.Use(async (context, next) =>
{
    context.Request.Headers["X-App-Source"] = "MVC";
    await next.Invoke();
});
app.UseMiddleware<LoggerMiddleware>();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

app.Run();
