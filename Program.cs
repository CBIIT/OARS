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
using Blazorise.Tailwind;
using Blazorise.Icons.FontAwesome;
using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Services;
using ITfoxtec.Identity.Saml2;
using ITfoxtec.Identity.Saml2.Schemas.Metadata;
using ITfoxtec.Identity.Saml2.MvcCore.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<AadService>();
builder.Services.AddSingleton<PbiEmbedService>();
builder.Services.AddScoped<PbiInterop>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<StudyService>();

// Add Blazorise and Tailwind UI
builder.Services.AddBlazorise();
builder.Services.AddTailwindProviders();
builder.Services.AddFontAwesomeIcons();

// Loading appsettings.json in C# Model classes
builder.Services.Configure<PowerBI>(builder.Configuration.GetSection("PowerBI"));
builder.Services.Configure<AzureAd>(builder.Configuration.GetSection("PowerBICredentials"));

// Load DB context
builder.Services.AddDbContextFactory<WrDbContext>(opt =>
    opt.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

/*
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    //options.CheckConsentNeeded = context => true;
    //options.MinimumSameSitePolicy = SameSiteMode.None;
    options.Secure = CookieSecurePolicy.Always;
});*/

// Okta authentication

builder.Services.AddAuthentication(authOptions =>
{
    authOptions.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    authOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    authOptions.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    authOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
}).AddOpenIdConnect(oidcOptions =>
{
    oidcOptions.ClientId = builder.Configuration["Okta:ClientId"];
    oidcOptions.ClientSecret = builder.Configuration["Okta:ClientSecret"];
    oidcOptions.CallbackPath = "/authorization-code/callback";
    oidcOptions.Authority = builder.Configuration["Okta:Issuer"];
    oidcOptions.ResponseType = "code";
    oidcOptions.SaveTokens = true;
    oidcOptions.Scope.Add("openid");
    oidcOptions.Scope.Add("profile");
    oidcOptions.TokenValidationParameters.ValidateIssuer = false;
    oidcOptions.TokenValidationParameters.NameClaimType = "name";
    //oidcOptions.RequireHttpsMetadata = false;
}).AddCookie();

/*}).AddCookie(options =>
{
    //options.Cookie.SameSite = SameSiteMode.Strict;
    //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
*/
/*
}).AddCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});*/

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
builder.Services.Configure<Saml2Configuration>(builder.Configuration.GetSection("Saml2"));

builder.Services.Configure<Saml2Configuration>(saml2Configuration =>
{
    saml2Configuration.AllowedAudienceUris.Add(saml2Configuration.Issuer);

    var entityDescriptor = new EntityDescriptor();
    entityDescriptor.ReadIdPSsoDescriptorFromUrl(new Uri(builder.Configuration["Saml2:IdPMetadata"]));
    if (entityDescriptor.IdPSsoDescriptor != null)
    {
        saml2Configuration.SingleSignOnDestination = entityDescriptor.IdPSsoDescriptor.SingleSignOnServices.First().Location;
        saml2Configuration.SignatureValidationCertificates.AddRange(entityDescriptor.IdPSsoDescriptor.SigningCertificates);
    }
    else
    {
        throw new Exception("IdPSsoDescriptor not loaded from metadata.");
    }
});

builder.Services.AddSaml2();
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

/*
app.UseCookiePolicy(new CookiePolicyOptions()
{
    MinimumSameSitePolicy = SameSiteMode.Strict
}) ;*/

app.UseStaticFiles();
app.UseRouting();
app.UseSaml2();

//app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
