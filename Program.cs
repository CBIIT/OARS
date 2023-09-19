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
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TheradexPortal.Data.Static;
using Microsoft.AspNetCore.Identity;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Identity;
using TheradexPortal.Data.Services.Abstract;
using TheradexPortal.Data.PowerBI.Abstract;
using ITfoxtec.Identity.Saml2;
using ITfoxtec.Identity.Saml2.Schemas.Metadata;
using ITfoxtec.Identity.Saml2.MvcCore.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<IAadService, AadService>();
builder.Services.AddSingleton<IPbiEmbedService, PbiEmbedService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
builder.Services.AddScoped<IStudyService, StudyService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IAlertService, AlertService>();
//builder.Services.AddHttpClient<IOktaService, OktaService>(client =>
//{
//    client.BaseAddress = new Uri(builder.Configuration["Okta:Issuer"]);
//    client.DefaultRequestHeaders.Add("Accept", "application/json")
//});
builder.Services.AddScoped<TimeZoneService>();

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

var onTokenValidated = async (TokenValidatedContext context) =>
{
    if (context is null || context.Principal is null || context.Principal.Identity is null)
        return Task.CompletedTask;


    // Add custom claims to the identity
    var claimsIdentity = (ClaimsIdentity)context.Principal.Identity;

    var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
    var userRoleService = context.HttpContext.RequestServices.GetRequiredService<IUserRoleService>();
    var dashboardService = context.HttpContext.RequestServices.GetRequiredService<IDashboardService>();

    var userIsRegistered = false;
    var roleList = "";

    if (claimsIdentity.Claims is null)
    {
        claimsIdentity.AddClaim(new Claim(WRClaimType.Registered, userIsRegistered.ToString()));
        return Task.CompletedTask;
    }
    Claim emailClaim = (claimsIdentity).Claims.Where(c => c.Type == "preferred_username").FirstOrDefault();
    if (emailClaim is null)
    {
        claimsIdentity.AddClaim(new Claim(WRClaimType.Registered, userIsRegistered.ToString()));
        return Task.CompletedTask;
    }
    var email = emailClaim.Value;
    if (email is null)
    {
        claimsIdentity.AddClaim(new Claim(WRClaimType.Registered, userIsRegistered.ToString()));
        return Task.CompletedTask;
    }

    var user = await userService.GetUserByEmailAsync(email);
    if (user is null)
    {
        claimsIdentity.AddClaim(new Claim(WRClaimType.Registered, userIsRegistered.ToString()));
        return Task.CompletedTask;
    }
    userIsRegistered = true;

    if(user.IsActive && !user.IsLockedOut)
    {
        claimsIdentity.AddClaim(new Claim(WRClaimType.Registered, userIsRegistered.ToString()));
    }

    claimsIdentity.AddClaim(new Claim(WRClaimType.UserId, user.UserId.ToString()));

    var userRoles = await userRoleService.GetUserRolesAsync(user.UserId);
    var isAdmin = false;
    var isContentAdmin = false;
    foreach(var role in userRoles)
    {        
        claimsIdentity.AddClaim(new Claim(WRClaimType.Role, role.RoleName));
        roleList += role.RoleName + ",";
        if (role.AdminType == WRAdminType.Super || role.AdminType == WRAdminType.Content)
            isAdmin = true;
        if (role.AdminType == WRAdminType.Content)
            isContentAdmin = true;
    }
    roleList = roleList.TrimEnd(',');

    var dashboardIds = await dashboardService.GetDashboardIdsForUser(user.UserId, isAdmin);
    var reportIds = await dashboardService.GetReportIdsForUser(user.UserId, isAdmin);

    claimsIdentity.AddClaim(new Claim(WRClaimType.IsAdmin, isAdmin.ToString()));
    claimsIdentity.AddClaim(new Claim(WRClaimType.IsContentAdmin, isContentAdmin.ToString()));
    claimsIdentity.AddClaim(new Claim(WRClaimType.Dashboards, dashboardIds));
    claimsIdentity.AddClaim(new Claim(WRClaimType.Reports, reportIds));

    // Save activity & reload starting studies
    bool updateLastLoginDate = userService.SaveLastLoginDate(user.UserId);
    bool saveActivity = userService.SaveActivityLog(user.UserId, WRActivityType.Login, roleList);
    int recentCount = Convert.ToInt32(builder.Configuration["System:RecentHistoryCount"]);
    bool setStartingStudies = await userService.SetStartingStudies(user.UserId, recentCount);
    return Task.CompletedTask;

};

// Add policies based on claims for Blazor auth checks
builder.Services.AddAuthorization(options =>
      options.AddPolicy("IsAdmin",
      policy => policy.RequireClaim(WRClaimType.IsAdmin, true.ToString())));

builder.Services.AddAuthorization(options =>
      options.AddPolicy("IsRegistered",
      policy => policy.RequireClaim(WRClaimType.Registered, true.ToString())));

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
    //oidcOptions.SignedOutRedirectUri = "/landing";
    oidcOptions.Authority = builder.Configuration["Okta:Issuer"];
    oidcOptions.ResponseType = "code";
    oidcOptions.SaveTokens = true;
    oidcOptions.Scope.Add("openid");
    oidcOptions.Scope.Add("profile");
    oidcOptions.TokenValidationParameters.ValidateIssuer = false;
    oidcOptions.TokenValidationParameters.NameClaimType = "name";
    oidcOptions.Events = new OpenIdConnectEvents
    {        
        OnTokenValidated = onTokenValidated
    };
})
.AddCookie();

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

app.UseStaticFiles();
app.UseRouting();
app.UseSaml2();

app.UseAuthentication();
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
