using Api.Content.Infrastructure.Extensions;
using AutoMapper;
using Coco.Framework.Infrastructure.Extensions;
using Coco.Business;
using Coco.Business.AutoMap;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Api.Content.Resolvers.Contracts;
using Api.Content.Resolvers;
using Api.Content.Infrastructure.AutoMap;

namespace Api.Content
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "AllowOrigin";

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
            services.ConfigureApplicationMapping();
            services.AddAutoMapper(typeof(IdentityMappingProfile), typeof(ContentMappingProfile));
            services.ConfigureApplicationServices();
            services.ConfigureBusinessServices();

            services.AddTransient<IImageResolver, ImageResolver>();
            services.AddTransient<IUserPhotoResolver, UserPhotoResolver>();

            services.ConfigureGraphQlServices();
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
