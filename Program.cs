using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rater.Data;
using Rater.Models;
using Azure.Identity;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("RaterKeyVaultUri"));
config.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());
var connectionString = config.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
}).AddEntityFrameworkStores<ApplicationDbContext>();
    
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication()
   .AddGoogle(options =>
   {
       options.ClientId = config["Authentication:Google:ClientId"];
       options.ClientSecret = config["Authentication:Google:ClientSecret"];
   });
   //.AddMicrosoftAccount(microsoftOptions =>
   //{
   //    microsoftOptions.ClientId = config["Authentication:Microsoft:ClientId"];
   //    microsoftOptions.ClientSecret = config["Authentication:Microsoft:ClientSecret"];
   //});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();
app.MapRazorPages();

app.Run();
