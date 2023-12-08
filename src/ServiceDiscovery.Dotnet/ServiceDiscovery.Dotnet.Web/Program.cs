using Microsoft.FluentUI.AspNetCore.Components;
using ServiceDiscovery.Dotnet.Web;
using ServiceDiscovery.Dotnet.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
Console.WriteLine(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
{
    builder.AddServiceDefaults();
    builder.AddRedisOutputCache("cache");
    builder.Services.AddHttpClient<GatewayApiClient>(client=> client.BaseAddress = new("http://gateway"));
}
else
{
    builder.Services.AddHttpClient<GatewayApiClient>(client=> client.BaseAddress = new("http://127.0.0.1:5024"));
}

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


builder.Services.AddFluentUIComponents();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.MapDefaultEndpoints();
}

app.UseStaticFiles();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
    
if (!app.Environment.IsDevelopment())
{
    app.UseOutputCache();
    app.MapDefaultEndpoints();
}

app.Run();
