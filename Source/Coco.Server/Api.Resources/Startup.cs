using Api.Public.Resolvers;
using Api.Public.Resolvers.Contracts;
using Api.Resource.Infrastructure.Extensions;
using AutoMapper;
using Coco.Framework.Infrastructure;
using Coco.Framework.MappingProfiles;
using Coco.Business;
using Coco.Business.MappingProfiles;
using Coco.Contract;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Api.Resource
{
    public class Startup
    {
        private IBootstrapper _bootstrapper;
        readonly string MyAllowSpecificOrigins = "AllowOrigin";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _bootstrapper = new BusinessStartup(configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Config AddCors
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins("http://localhost:3000",
                        "http://localhost:7000",
                        "http://localhost:5000",
                        "http://localhost:45678")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            InvokeInitialStartup(services, Configuration);
            services.AddControllers()
                .AddNewtonsoftJson();
        }

        private void InvokeInitialStartup(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(FrameworkMappingProfile), typeof(UserMappingProfile));
            FrameworkStartup.AddCustomStores(services);
            _bootstrapper.RegiserTypes(services);

            services.AddTransient<IImageResolver, ImageResolver>();
            services.AddGraphQlDependency();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection()
                .UseRouting()
                .UseCors(MyAllowSpecificOrigins)
                .UseAuthentication()
                .UseAuthorization()
                .UseWebSockets()
                .UseGraphQL("/api/graphql")
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
        }
    }
}
