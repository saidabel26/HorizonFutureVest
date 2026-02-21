using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using Persistence.Repositories;
using Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuración de servicios para Razor Pages y MVC
builder.Services.AddControllersWithViews();

// Configuración de EF Core con SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));

// Registro de repositorios
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IMacroIndicatorRepository, MacroIndicatorRepository>();
builder.Services.AddScoped<ICountryIndicatorRepository, CountryIndicatorRepository>();
builder.Services.AddScoped<IReturnRateConfigRepository, ReturnRateConfigRepository>();

// Registro de servicios de aplicación
builder.Services.AddScoped<IRankingService, RankingService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IMacroIndicatorService, MacroIndicatorService>();
builder.Services.AddScoped<ICountryIndicatorService, CountryIndicatorService>();
builder.Services.AddScoped<IReturnRateConfigService, ReturnRateConfigService>();

var app = builder.Build();

// Configuración del pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
