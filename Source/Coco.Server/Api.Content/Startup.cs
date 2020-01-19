using Api.Content.Infrastructure.Extensions;
using AutoMapper;
using Coco.Api.Framework.Infrastructure;
using Coco.Api.Framework.Infrastructure.Extensions;
using Coco.Api.Framework.MappingProfiles;
using Coco.Business;
using Coco.Business.MappingProfiles;
using Coco.Contract;
using GraphiQl;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Api.Content
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "AllowOrigin";
        private IBootstrapper _bootstrapper;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _bootstrapper = new BusinessStartup(configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins("http://localhost:3000",
                        "http://localhost:5000",
                        "http://localhost:45678")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            InvokeInitialStartup(services);
            services.AddControllers()
                .AddNewtonsoftJson();
        }

        private void InvokeInitialStartup(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(FrameworkMappingProfile), typeof(UserMappingProfile));
            FrameworkStartup.AddCustomStores(services);
            _bootstrapper.RegiserTypes(services);

            #region GraphQL DI
            services.AddGraphQlDependency();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // Config UseCors
            app.UseHttpsRedirection()
                .UseRouting()
                .UseCors(MyAllowSpecificOrigins)
                .UseGraphiQl("/api/graphql")
                .UseBasicApiMiddleware();
        }
    }
}
