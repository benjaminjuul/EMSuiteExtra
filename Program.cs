using EMSuite.Data;
using EMSuite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ComponentSpace.Saml2.Configuration.Database;
using ComponentSpace.Saml2.Configuration.Resolver;
using EMSuite.ViewModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<EMSuiteContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        builder => builder.MigrationsAssembly("EMSuite")));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();


builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // SameSiteMode.None is required to support SAML SSO
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Use a unique identity cookie name rather than sharing the cookie across applications in the domain.
    options.Cookie.Name = "EMSuite.Identity";

    // SameSiteMode.None is required to support SAML logout.
    options.Cookie.SameSite = SameSiteMode.None;
});

// Add the SAML configuration database context.
builder.Services.AddDbContext<SamlConfigurationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SamlConfigurationConnection"),
        builder => builder.MigrationsAssembly("EMSuite")));


// Add SAML SSO services.
builder.Services.AddSaml();

var cacheSamlConfiguration = builder.Configuration.GetValue<bool>("CacheSamlConfiguration");

if (cacheSamlConfiguration)
{
    // Use the cached resolver backed by the database configuration resolver.
    builder.Services.AddTransient<ISamlConfigurationResolver, SamlCachedConfigurationResolver>();

    builder.Services.AddTransient<SamlDatabaseConfigurationResolver>();

    builder.Services.Configure<SamlCachedConfigurationResolverOptions>(options =>
    {
        options.CacheSamlConfigurationResolver<SamlDatabaseConfigurationResolver>();
        options.MemoryCacheEntryOptions = (key, value, memoryCacheEntryOptions) =>
        {
            memoryCacheEntryOptions.AbsoluteExpirationRelativeToNow = builder.Configuration.GetValue<TimeSpan?>("SamlCacheExpiration");
        };
    });
}
else
{
    // Use the database configuration resolver.
    builder.Services.AddTransient<ISamlConfigurationResolver, SamlDatabaseConfigurationResolver>();
}


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
