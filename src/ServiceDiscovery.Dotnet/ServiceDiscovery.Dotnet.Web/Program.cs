using Fluxor;
using MassTransit;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Components.Tooltip;
using ServiceDiscovery.Dotnet.Shared;
using ServiceDiscovery.Dotnet.Web;
using ServiceDiscovery.Dotnet.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.

builder.AddRedisOutputCache("cache");
if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
{
	builder.AddServiceDefaults();
	builder.Services.AddHttpClient<GatewayApiClient>(client => client.BaseAddress = new("http://gateway/v1"));
}
else
{
	builder.Services.AddHttpClient<GatewayApiClient>(client => client.BaseAddress = new("http://127.0.0.1:5024/v1"));
	builder.Services.AddHttpClient("Gateway", client => client.BaseAddress = new("http://127.0.0.1:5024/v1"));
	builder.Services.AddMassTransit(x =>
	{
		x.SetKebabCaseEndpointNameFormatter();
		x.UsingRabbitMq((context, cfg) =>
		{
			cfg.Host("localhost", "/", h =>
			{
				h.Username("guest");
				h.Password("guest");
			});
		});
	});
}

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

// Fluxor State
builder.Services.AddFluxor(options =>
    options.ScanAssemblies(typeof(Program).Assembly, [typeof(BaseState).Assembly])
);
// FluentUI
builder.Services.AddScoped<ITooltipService, TooltipService>();
builder.Services.AddFluentUIComponents();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	_ = app.MapDefaultEndpoints();
}

app.UseStaticFiles();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseOutputCache();
if (!app.Environment.IsDevelopment())
{
	app.MapDefaultEndpoints();
}

app.Run();
