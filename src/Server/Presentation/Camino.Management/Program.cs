using Camino.Framework.Infrastructure.Extensions;
using Camino.Framework.Infrastructure.Middlewares;
using Camino.Framework.Infrastructure.ModelBinders;
using Camino.Infrastructure.Infrastructure.Extensions;
using Camino.Management.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.ConfigureManagementServices(builder.Configuration)
    .AddControllersWithViews(options =>
    {
        options.ModelBinderProviders.Insert(0, new ApplicationModelBinderProvider());
    })
    .AddNewtonsoftJson()
    .AddModular();

builder.Services.AddAutoMappingModular();

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