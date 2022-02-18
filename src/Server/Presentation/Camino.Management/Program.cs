using Camino.Framework.Infrastructure.Middlewares;
using Camino.Management.Extensions.DependencyInjection;
using Camino.Management.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.ConfigureManagementServices(builder.Configuration);

// Configure application
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseManagementConfiguration();
app.UseModular(app.Environment);

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});
app.Run();