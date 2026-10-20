using provider.Endpoints;
using provider.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSingleton<CarStore>();

var app = builder.Build();

app.MapOpenApi();
app.UseHttpsRedirection();
app.MapCarEndpoints();

app.Run();
