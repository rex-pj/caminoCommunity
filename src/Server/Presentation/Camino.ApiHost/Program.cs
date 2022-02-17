using Camino.ApiHost.Extensions.DependencyInjection;
using Camino.ApiHost.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.ConfigureApiHostServices(builder.Configuration);

// Configure application
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.ConfigureAppBuilder(app.Environment);
app.Run();