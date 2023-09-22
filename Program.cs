using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rater.Data;
using Rater.Models;
using System.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Data.SqlClient;
using Azure.Identity;
using Azure.Core;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Azure;
using System.Diagnostics;
using System.Net;
using Microsoft.CodeAnalysis.Completion;
using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Rater.Controllers;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

config.AddAzureAppConfiguration(options =>
{   
    options.Connect(config.GetConnectionString("AppConfig"))
    .ConfigureKeyVault(kv =>
    {
        kv.SetCredential(new DefaultAzureCredential());
    });
});

var client = new SecretClient(
    new Uri(config["SecretsUri"]!),
    new DefaultAzureCredential());

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(config.GetConnectionString("Database")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = config["AllowedCharacters"]!;
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
}).AddEntityFrameworkStores<ApplicationDbContext>()
.AddRoles<IdentityRole<int>>()
.AddDefaultUI();

var searchServiceUri = config["searchUrl"];
var queryApiKey = config["SearchServiceQueryApiKey"];
var indexName = config["searchIndexName"];
SearchController.Initialize(searchServiceUri!, queryApiKey!, indexName!);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = config["Authentication:Google:ClientId"]!;
        options.ClientSecret = client.GetSecret("AuthenticationGoogleClientSecret").Value.Value;
    })
    .AddMicrosoftAccount(microsoftOptions =>
    {
        microsoftOptions.ClientId = config["Authentication:Microsoft:ClientId"]!;
        microsoftOptions.ClientSecret = client.GetSecret("AuthenticationMicrosoftClientSecret").Value.Value;
    });

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

app.UseCookiePolicy();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();
app.MapRazorPages();

using(var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
    string role = "Administrator";
    
    if (!await roleManager.RoleExistsAsync(role))
        await roleManager.CreateAsync(new IdentityRole<int>(role));
}

app.Run();