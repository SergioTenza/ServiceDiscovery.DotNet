var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddProblemDetails();
var app = builder.Build();
app.UseExceptionHandler();

app.MapGet("/", () => "Hello Mario! The princess is in another Castle.");

app.MapDefaultEndpoints();

app.Run();
