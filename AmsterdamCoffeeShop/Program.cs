using Microsoft.EntityFrameworkCore;
using AmsterdamCoffeeShop.Models;
using Azure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add Azure Key Vault configuration if not in development environment
if (!builder.Environment.IsDevelopment())
{
    var keyVaultName = builder.Configuration["KeyVault:Name"];
    if (!string.IsNullOrEmpty(keyVaultName))
    {
        var keyVaultUri = new Uri($"https://{keyVaultName}.vault.azure.net/");
        
        builder.Configuration.AddAzureKeyVault(
            keyVaultUri,
            new DefaultAzureCredential());
    }
}

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add Application Insights
builder.Services.AddApplicationInsightsTelemetry();

// Add session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();

app.MapRazorPages();

// Ensure database is created and seeded when in development
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var dbContext = services.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.EnsureCreated();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while creating/seeding the database.");
        }
    }
}

app.Run();
