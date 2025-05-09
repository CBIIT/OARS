using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.Net.Http.Headers;
using OARS.Data;
using OARS.Data.PowerBI;
using OARS.Data.PowerBI.Models;
using OARS.Data.Models.Configuration;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Microsoft.EntityFrameworkCore;
using OARS.Data.Services;
using System.Security.Claims;
using OARS.Data.Static;
using OARS.Data.Services.Abstract;
using OARS.Data.PowerBI.Abstract;
using ITfoxtec.Identity.Saml2;
using ITfoxtec.Identity.Saml2.Schemas.Metadata;
using ITfoxtec.Identity.Saml2.MvcCore.Configuration;
using Microsoft.Extensions.Logging;
using Amazon.DynamoDBv2;
using Microsoft.Extensions.DependencyInjection;
using Amazon.DynamoDBv2.DataModel;
using Amazon.S3;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.HttpLogging;
using OARS.Middleware;
using System.Diagnostics;
using OARS.Data.Services.Abstract.ADDR;
using OARS.Pages.ADDR;
using OARS.Data.Models.ADDR;
using OARS.Data.Services.Pharma;
using OARS.Data.Services.Abstract;
using OARS.Data.Services.Abstract.Pharma;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.RequestMethod
                                | HttpLoggingFields.ResponseStatusCode
                                | HttpLoggingFields.RequestPath;
});

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<IAadService, AadService>();
builder.Services.AddSingleton<IPbiEmbedService, PbiEmbedService>();
builder.Services.AddScoped<IDatabaseConnectionService, DatabaseConnectionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
builder.Services.AddScoped<IStudyService, StudyService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IAlertService, AlertService>();
builder.Services.AddScoped<IContactUsService, ContactUsService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IErrorLogService, ErrorLogService>();
builder.Services.AddScoped<IUploadService, UploadService>();
builder.Services.AddScoped<IThorCategoryService, ThorCategoryService>();
builder.Services.AddScoped<IThorFieldService, ThorFieldService>();
builder.Services.AddScoped<IThorDictionaryService, ThorDictionaryService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IProtocolMappingService, ProtocolMappingService>();
builder.Services.AddScoped<IProfileCategoryService, ProfileCategoryService>();
builder.Services.AddScoped<IProtocolDataSystemService, ProtocolDataSystemService>();
builder.Services.AddScoped<IProtocolFieldService, ProtocolFieldService>();
builder.Services.AddScoped<IProfileFieldService, ProfileFieldService>();
builder.Services.AddScoped<IProtocolEDCFieldService, ProtocolEDCFieldService>();
builder.Services.AddScoped<IProtocolEDCFormService, ProtocolEDCFormService>();
builder.Services.AddScoped<IProtocolEDCDictionaryService, ProtocolEDCDictionaryService>();
builder.Services.AddScoped<IALSFileImportService, ALSFileImportService>();
builder.Services.AddScoped<IXMLFileImportService, XMLFileImportService>();
builder.Services.AddScoped<ICSVFileImportService, CSVFileImportService>();
builder.Services.AddScoped<IProtocolPhaseService, ProtocolPhaseService>();
builder.Services.AddScoped<IProtocolEDCFormService, ProtocolEDCFormService>();
builder.Services.AddScoped<IProtocolAgentService, ProtocolAgentService>();
builder.Services.AddScoped<IProtocolSubGroupService, ProtocolSubGroupService>();
builder.Services.AddScoped<IProtocolTACService, ProtocolTACService>();
builder.Services.AddScoped<IProtocolDiseaseService, ProtocolDiseaseService>();
builder.Services.AddScoped<IProtocolDataCategoryService, ProtocolDataCategoryService>();
builder.Services.AddScoped<IProtocolFieldMappingService, ProtocolFieldMappingService>();
builder.Services.AddScoped<IProtocolDictionaryMappingService, ProtocolDictionaryMappingService>();
builder.Services.AddScoped<IProtocolFormMappingService, ProtocolFormMappingService>();
builder.Services.AddScoped<IReviewItemService, ReviewItemService>();
builder.Services.AddScoped<IReviewHistoryService, ReviewHistoryService>();
builder.Services.AddScoped<IReviewHistoryNoteService, ReviewHistoryNoteService>();
builder.Services.AddScoped<IReviewHistoryEmailService, ReviewHistoryEmailService>();
builder.Services.AddScoped<IReviewHistoryItemService, ReviewHistoryItemService>();
builder.Services.AddScoped<IReviewService, ReviewService>();

builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddScoped<IPharmaNscTacService, PharmaNscTacService>();
builder.Services.AddScoped<IPharmaCdrdmStudyAgentService, PharmaCdrdmStudyAgentService>();
builder.Services.AddScoped<IPharmaProtocolTacService, PharmaProtocolTacService>();


#region Automated Data Discrepancy Report (ADDR)
//builder.Services.AddScoped<IADDRService, ADDRService>();

builder.Services.AddScoped<IReceivingStatusService, ReceivingStatusService>(); // Register with IReceivingStatusService interface
builder.Services.AddScoped<INotesService<ReceivingStatus>, ReceivingStatusService>(); // Register with INotesService interface
builder.Services.AddScoped<INotesService<ShippingStatus>, ShippingStatusService>(); // Register with INotesService interface
builder.Services.AddScoped<IShippingStatusService, ShippingStatusService>(); // Register with IReceivingStatusService interface

//builder.Services.AddScoped<IReceivingStatusService, ReceivingStatusService>();
//builder.Services.AddScoped<IShippingStatusService, ShippingStatusService>();
#endregion

builder.Services.AddHttpClient<IOktaService, OktaService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Okta:Issuer"]);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    client.DefaultRequestHeaders.Add("Authorization", "SSWS " + builder.Configuration["Okta:ApiKey"]);
});

builder.Services.AddScoped<TimeZoneService>();


builder.Host.ConfigureLogging((context, logging) =>
{
    builder.Logging.ClearProviders();
    builder.Logging.AddLog4Net("log4net.config");
});

builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());

//builder.Logging.ClearProviders();

// Add Blazorise and Tailwind UI
builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();

// Load email settings
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// Load upload settings
builder.Services.Configure<UploadSettings>(builder.Configuration.GetSection("UploadSettings"));

// Loading appsettings.json in C# Model classes
builder.Services.Configure<PowerBI>(builder.Configuration.GetSection("PowerBI"));
builder.Services.Configure<AzureAd>(builder.Configuration.GetSection("PowerBICredentials"));

// Load DB context
//builder.Services.AddDbContextFactory<ThorDBContext>(opt =>
//    opt.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection"), oracleOptions => oracleOptions.CommandTimeout(600)));

builder.Services.AddDbContextFactory<ThorDBContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection"), oracleOptions => oracleOptions.CommandTimeout(600)));


// Load DynamodB context
builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddSingleton<IDynamoDBContext, DynamoDBContext>(p => new DynamoDBContext(new AmazonDynamoDBClient()));
builder.Services.AddTransient<IDynamoDbService, DynamoDbService>();

// Load S3
builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddTransient<IAWSS3Service, AWSS3Service>();

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
        claimsIdentity.AddClaim(new Claim(ThorClaimType.Registered, userIsRegistered.ToString()));
        return Task.CompletedTask;
    }
    Claim emailClaim = (claimsIdentity).Claims.Where(c => c.Type == "preferred_username").FirstOrDefault();
    if (emailClaim is null)
    {
        claimsIdentity.AddClaim(new Claim(ThorClaimType.Registered, userIsRegistered.ToString()));
        return Task.CompletedTask;
    }
    var email = emailClaim.Value;
    if (email is null)
    {
        claimsIdentity.AddClaim(new Claim(ThorClaimType.Registered, userIsRegistered.ToString()));
        return Task.CompletedTask;
    }

    var user = await userService.GetUserByEmailAsync(email);
    if (user is null)
    {
        claimsIdentity.AddClaim(new Claim(ThorClaimType.Registered, userIsRegistered.ToString()));
        return Task.CompletedTask;
    }
    userIsRegistered = true;

    if (user.IsActive)
    {
        claimsIdentity.AddClaim(new Claim(ThorClaimType.Registered, userIsRegistered.ToString()));
    }

    claimsIdentity.AddClaim(new Claim(ThorClaimType.UserId, user.UserId.ToString()));

    var userRoles = await userRoleService.GetUserRolesAsync(user.UserId);
    var isAdmin = false;
    var isDMUAdmin = false;
    var isAnyAdmin = false;

    foreach (var role in userRoles)
    {
        claimsIdentity.AddClaim(new Claim(ThorClaimType.Role, role.RoleName));
        roleList += role.RoleName + ",";
        if (role.AdminType == ThorAdminType.IT || role.AdminType == ThorAdminType.Biz || role.AdminType == ThorAdminType.Content)
        {
            isAdmin = true;
            if (!claimsIdentity.HasClaim(c => c.Type == "Admin-" + role.AdminType))
            {
                claimsIdentity.AddClaim(new Claim("Admin-" + role.AdminType, "true"));
            }
        }

        if (role.AdminType == ThorAdminType.DMUGlobal || role.AdminType == ThorAdminType.DMUStudy)
        {
            isDMUAdmin = true;
            if (!claimsIdentity.HasClaim(c => c.Type == "Admin-" + role.AdminType))
            {
                claimsIdentity.AddClaim(new Claim("Admin-" + role.AdminType, "true"));
            }
        }

        isAnyAdmin = isAdmin || isDMUAdmin;

    }
    roleList = roleList.TrimEnd(',');

    var dashboardIds = await dashboardService.GetDashboardIdsForUser(user.UserId, isAdmin);
    var reportIds = await dashboardService.GetReportIdsForUser(user.UserId, isAdmin);

    claimsIdentity.AddClaim(new Claim(ThorClaimType.IsAdmin, isAdmin.ToString()));
    claimsIdentity.AddClaim(new Claim(ThorClaimType.IsDMUAdmin, isDMUAdmin.ToString()));
    claimsIdentity.AddClaim(new Claim(ThorClaimType.IsAnyAdmin, isAnyAdmin.ToString()));
    claimsIdentity.AddClaim(new Claim(ThorClaimType.Dashboards, dashboardIds));
    claimsIdentity.AddClaim(new Claim(ThorClaimType.Reports, reportIds));

    // Save activity & reload starting studies
    bool updateLastLoginDate = userService.SaveLastLoginDate(user.UserId);
    bool saveActivity = userService.SaveActivityLog(user.UserId, ThorActivityType.Login, roleList);
    int recentCount = Convert.ToInt32(builder.Configuration["System:RecentHistoryCount"]);
    bool setStartingStudies = await userService.SetStartingStudies(user.UserId, recentCount);
    log4net.ThreadContext.Properties["userName"] = user.UserId;
    log4net.LogicalThreadContext.Properties["userName"] = user.UserId;
    log4net.ThreadContext.Properties["email"] = user.EmailAddress;
    log4net.LogicalThreadContext.Properties["email"] = user.EmailAddress;
    return Task.CompletedTask;

};

// Add policies based on claims for Blazor auth checks
builder.Services.AddAuthorization(options =>
      options.AddPolicy("IsAdmin",
      policy => policy.RequireClaim(ThorClaimType.IsAdmin, true.ToString())));

builder.Services.AddAuthorization(options =>
      options.AddPolicy("IsDMUAdmin",
      policy => policy.RequireClaim(ThorClaimType.IsDMUAdmin, true.ToString())));

builder.Services.AddAuthorization(options =>
      options.AddPolicy("IsAnyAdmin",
      policy => policy.RequireClaim(ThorClaimType.IsAnyAdmin, true.ToString())));

builder.Services.AddAuthorization(options =>
      options.AddPolicy("IsRegistered",
      policy => policy.RequireClaim(ThorClaimType.Registered, true.ToString())));

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
app.UseCorrelationIdMiddleware();
//app.UseUserIdHeaderMiddleware();
app.UseLog4NetTraceIdMiddleware();
app.UseExceptionHandlingMiddleware();
app.UseLogResponseMiddleware();
//app.UseRequestLoggingMiddleware();
app.UseElapsedTimeMiddleware();
app.UseHttpLogging();
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
