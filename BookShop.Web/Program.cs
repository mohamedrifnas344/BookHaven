using BookShop.Domain.Entities;
using BookShop.Infrastructure;
using BookShop.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.InfrastructureConfiguration(builder.Configuration);

builder.Services.AddIdentity<AppUser, IdentityRole>(option =>
{
    option.Password.RequiredLength = 6;
    option.Password.RequireUppercase = false;
    option.Password.RequireLowercase = false;
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequireDigit = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    try
    {
        var userManager = service.GetRequiredService<UserManager<AppUser>>();
        var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
        var loggerFactory = service.GetRequiredService<ILoggerFactory>();

        await IdentitySeeder.LoadData(userManager, roleManager, loggerFactory);
    }
    catch (Exception ex)
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occured while seeding identity data");
    }
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

StripeConfiguration.ApiKey = builder.Configuration.GetSection("StripeSettings:SecretKey").Get<string>();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
