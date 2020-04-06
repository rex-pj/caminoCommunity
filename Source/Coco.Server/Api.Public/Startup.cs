using Api.Public.Infrastructure.Extensions;
using Api.Public.Resolvers;
using Api.Public.Resolvers.Contracts;
using AutoMapper;
using Coco.Api.Framework.Infrastructure;
using Coco.Api.Framework.MappingProfiles;
using Coco.Api.Framework.SessionManager.Core;
using Coco.Business;
using Coco.Business.MappingProfiles;
using Coco.Contract;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Api.Public
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

            InvokeInitialStartup(services);
            services.AddControllers()
                .AddNewtonsoftJson();
        }

        private void InvokeInitialStartup(IServiceCollection services)
        {
            _bootstrapper.RegiserTypes(services);
            FrameworkStartup.AddCustomStores(services);
            services.AddAutoMapper(typeof(FrameworkMappingProfile), typeof(UserMappingProfile));

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // User settings.
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            });

            services.AddTransient<IUserResolver, UserResolver>();
            services.AddTransient<ICountryResolver, CountryResolver>();
            services.AddTransient<IGenderResolver, GenderResolver>();

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
