using Camino.ApiHost.Extensions.DependencyInjection;
using Camino.ApiHost.Middlewares;
using Camino.Infrastructure.Modularity.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.ConfigureApiHostServices(builder.Configuration);

// Configure application
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHttpsRedirection();
}

app.ConfigureAppBuilder();
app.UseModular(app.Environment);
app.Run();