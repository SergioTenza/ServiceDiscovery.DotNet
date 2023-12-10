using System.Text.RegularExpressions;
using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ServiceDiscovery.Dotnet.ApiGateway;
using ServiceDiscovery.Dotnet.Shared;
using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.Model;

const string DEBUG_HEADER = "Debug";
const string DEBUG_METADATA_KEY = "debug";
const string DEBUG_VALUE = "true";

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddReverseProxy()
    .LoadFromMemory([], [])
    .LoadFromRedis(builder.Configuration);
builder.Services.AddSingleton(new List<RouteConfig>());
builder.Services.AddSingleton(new List<ClusterConfig>());
builder.Services.AddMassTransit(x =>
    {
        x.SetKebabCaseEndpointNameFormatter();
        x.AddConsumer<UpdateGatewayConsumer>();
        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host("localhost", "/", h =>
            {
                h.Username("guest");
                h.Password("guest");
            });
        });
    });
var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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
    .Routes()
    .Clusters();



app.Map("/update", context =>
{
    //TODO: Move to get actual data from Redis
    context.RequestServices.GetRequiredService<InMemoryConfigProvider>().Update(GetRoutes(), GetClusters());
    return Task.CompletedTask;
});

app.Run();

RouteConfig[] GetRoutes()
{
    return
    [
        new RouteConfig()
        {
            RouteId = "route" + Random.Shared.Next(), // Forces a new route id each time GetRoutes is called.
            ClusterId = "cluster1",
            Match = new RouteMatch
            {
                // Path or Hosts are required for each route. This catch-all pattern matches all request paths.
                Path = "{**catch-all}"
                
            }
        }
    ];
}
ClusterConfig[] GetClusters()
{
    var debugMetadata = new Dictionary<string, string>
    {
        { DEBUG_METADATA_KEY, DEBUG_VALUE }
    };

    return
    [
        new ClusterConfig()
        {
            ClusterId = "cluster1",
            SessionAffinity = new SessionAffinityConfig { Enabled = true, Policy = "Cookie", AffinityKeyName = ".Yarp.ReverseProxy.Affinity" },
            Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase)
            {
                { "destination1", new DestinationConfig() { Address = "https://google.com" } },
                { "debugdestination1", new DestinationConfig() {
                    Address = "https://bing.com",
                    Metadata = debugMetadata  }
                },
            }
        }
    ];
}
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
            if (d.DestinationId.Contains("debug") == useDebugDestinations) { filteredDestinations.Add(d); }
        }
        availableDestinationsFeature.AvailableDestinations = filteredDestinations;
    }
    // Important - required to move to the next step in the proxy pipeline
    return next();
}


