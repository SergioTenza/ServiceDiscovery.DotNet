using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceDiscovery.Dotnet.ApiGateway;
using ServiceDiscovery.Dotnet.ApiGateway.IdentityContext;
using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.Model;

const string DEBUG_HEADER = "Debug";
const string DEBUG_METADATA_KEY = "debug";
const string DEBUG_VALUE = "true";

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddCors();
builder.Services.AddAuthorization();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddReverseProxy()
	.LoadFromMemory([], []);



builder.Services.AddDbContext<ApplicationDbContext>(
options => options.UseInMemoryDatabase("AppDb"));
builder.Services.AddMassTransit(x =>
	{
		x.SetKebabCaseEndpointNameFormatter();
		var assembly = typeof(Program).Assembly;
		x.AddConsumers(assembly);
		x.UsingRabbitMq((context, cfg) =>
		{
			var connectionstring = builder.Configuration.GetConnectionString("queue");
			cfg.Host(connectionstring);
			cfg.ConfigureEndpoints(context);
		});
	});



builder.Services.AddIdentityApiEndpoints<IdentityUser>()
	.AddEntityFrameworkStores<ApplicationDbContext>();
var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger().UseAuthorization();
	app.UseSwaggerUI().UseAuthorization();
}
app.UseCors(x =>
{
	x.WithOrigins(["http://127.0.0.1:5290"]);
});
app.UseHttpsRedirection();
app.MapReverseProxy(proxyPipeline =>
{
	// Use a custom proxy middleware, defined below
	proxyPipeline.Use(CustomProxyStep);
	// Don't forget to include these two middleware when you make a custom proxy pipeline (if you need them).
	proxyPipeline.UseSessionAffinity();
	proxyPipeline.UseLoadBalancing();
});

app.MapGroup("/v1")
	.Gateway()
	.Routes()
	.Clusters()
	.RequireAuthorization();

app.MapPost("/logout", async (SignInManager<IdentityUser> signInManager,
	[FromBody] object empty) =>
{
	if (empty != null)
	{
		await signInManager.SignOutAsync().ConfigureAwait(true);
		return Results.Ok();
	}
	return Results.Unauthorized();
})
.WithOpenApi()
.RequireAuthorization();

app.Map("/update", context =>
{
	//TODO: Move to get actual data from Redis
	var configProvider = context.RequestServices.GetRequiredService<InMemoryConfigProvider>();
	configProvider.Update([], []);
	return Task.CompletedTask;
})
.RequireAuthorization("Admin");

app.MapIdentityApi<IdentityUser>();

app.Run();

// RouteConfig[] GetRoutes()
// {
//     return
//     [
//         new RouteConfig()
//         {
//             RouteId = "route" + Random.Shared.Next(), // Forces a new route id each time GetRoutes is called.
//             ClusterId = "cluster1",
//             Match = new RouteMatch
//             {
//                 // Path or Hosts are required for each route. This catch-all pattern matches all request paths.
//                 Path = "{**catch-all}"

//             }
//         }
//     ];
// }
// ClusterConfig[] GetClusters()
// {
//     var debugMetadata = new Dictionary<string, string>
//     {
//         { DEBUG_METADATA_KEY, DEBUG_VALUE }
//     };

//     return
//     [
//         new ClusterConfig()
//         {
//             ClusterId = "cluster1",
//             SessionAffinity = new SessionAffinityConfig { Enabled = true, Policy = "Cookie", AffinityKeyName = ".Yarp.ReverseProxy.Affinity" },
//             Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase)
//             {
//                 { "destination1", new DestinationConfig() { Address = "https://google.com" } },
//                 { "debugdestination1", new DestinationConfig() {
//                     Address = "https://bing.com",
//                     Metadata = debugMetadata  }
//                 },
//             }
//         }
//     ];
// }
Task CustomProxyStep(HttpContext context, Func<Task> next)
{
	// Can read data from the request via the context
	var useDebugDestinations = context.Request.Headers.TryGetValue(DEBUG_HEADER, out var headerValues) && headerValues.Count == 1 && headerValues[0] == DEBUG_VALUE;

	// The context also stores a ReverseProxyFeature which holds proxy specific data such as the cluster, route and destinations
	var availableDestinationsFeature = context.Features.Get<IReverseProxyFeature>();
	var filteredDestinations = new List<DestinationState>();
	if (availableDestinationsFeature is not null)
	{
		// Filter destinations based on criteria
		foreach (var d in availableDestinationsFeature.AvailableDestinations)
		{
			//Todo: Replace with a lookup of metadata - but not currently exposed correctly here
			if (d.DestinationId.Contains(DEBUG_METADATA_KEY) == useDebugDestinations)
			{ filteredDestinations.Add(d); }
		}
		availableDestinationsFeature.AvailableDestinations = filteredDestinations;
	}
	// Important - required to move to the next step in the proxy pipeline
	return next();
}


