using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using System.Text.Json;
using TheradexPortal.Data;
using TheradexPortal.Data.PowerBI;
using TheradexPortal.Data.PowerBI.Models;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<AadService>();
builder.Services.AddSingleton<PbiEmbedService>();
builder.Services.AddScoped<PbiInterop>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<StudyService>();
builder.Services.AddSingleton<DashboardService>();

// Add Blazorise and Tailwind UI
builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();

// Loading appsettings.json in C# Model classes
builder.Services.Configure<PowerBI>(builder.Configuration.GetSection("PowerBI"));
builder.Services.Configure<AzureAd>(builder.Configuration.GetSection("PowerBICredentials"));

// Load DB context
builder.Services.AddDbContextFactory<WrDbContext>(opt =>
    opt.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));


// Configure Cognito auth
//var sessionCookieLifetime = builder.Configuration.GetValue("SessionCookieLifetimeMinutes", 60);
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
//})
//.AddCookie(options => {
//    options.ExpireTimeSpan = TimeSpan.FromMinutes(sessionCookieLifetime);
//})
//.AddOpenIdConnect(options =>
//{
//    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

//    options.Authority = builder.Configuration.GetValue<string>("CognitoConfig:Authority");
//    options.ClientId = builder.Configuration.GetValue<string>("CognitoConfig:ClientId");
//    options.ClientSecret = builder.Configuration.GetValue<string>("CognitoConfig:ClientSecret");

//    options.ResponseType = OpenIdConnectResponseType.Code;
//    options.SaveTokens = false;
//    options.GetClaimsFromUserInfoEndpoint = true;
//    options.Scope.Add("openid");
//    options.Scope.Add("profile");
//    options.Scope.Add("email");

//});

builder.Services.AddHttpContextAccessor();


var app = builder.Build();

// Force HTTPS context for use behind load balancer
app.Use((context, next) =>
{
    context.Request.Scheme = "https";
    return next(context);
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();

//app.UseAuthentication();
//app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
