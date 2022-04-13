using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Configuration;
using TheradexPortal.Data;
using TheradexPortal.Data.PowerBI;
using TheradexPortal.Data.PowerBI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<AadService>();
builder.Services.AddSingleton<PbiEmbedService>();
builder.Services.AddScoped<PbiInterop>();

// Loading appsettings.json in C# Model classes
builder.Services.Configure<PowerBI>(builder.Configuration.GetSection("PowerBI"));

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
