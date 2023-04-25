using Camino.Infrastructure.Modularity.Extensions;
using Camino.Management.Extensions.DependencyInjection;
using Camino.Management.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Camino.Infrastructure.Serilog.Extensions;


var builder = WebApplication.CreateBuilder(args);
SerilogConfig.BootstrapSerilog(builder.Configuration);

// Configure services
builder.Services.ConfigureManagementServices(builder.Configuration);

// Confugure Serilog
builder.Host.Configurelog();

// Configure application
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHttpsRedirection();
}

app.ConfigureAppBuilder();
app.UseModular(app.Environment);

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});
app.Run();