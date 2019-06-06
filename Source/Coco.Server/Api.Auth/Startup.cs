using Coco.Business;
using Coco.Contract;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GraphiQl;
using GraphQL;
using Api.Auth.GraphQLMutations;
using GraphQL.Types;
using Api.Auth.GraphQLSchema;
using Api.Auth.GraphQLTypes.InputTypes;
using Api.Auth.GraphQLTypes.ResultTypes;
using Api.Auth.GraphQLQueries;
using Coco.Api.Framework.AccountIdentity.Entities;
using Coco.Api.Framework;
using Api.Auth.GraphQLResolver;

namespace Api.Auth
{
    public class Startup
    {
        private IBootstrapper _bootstrapper;
        readonly string MyAllowSpecificOrigins = "AllowOrigin";
        private IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            _bootstrapper = new BusinessStartup(configuration);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Config AddCors
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins("http://localhost:3000")
                        .AllowAnyMethod()
                        //.WithExposedHeaders("HeaderUserHash")
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            InvokeInitialStartup(services, _configuration);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        private void InvokeInitialStartup(IServiceCollection services, IConfiguration configuration)
        {
            FrameworkStartup.AddCustomStores(services);

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                options.Stores.ShouldProtectPersonalData = true;

                // User settings.
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            _bootstrapper.RegiserTypes(services);

            #region GraphQL DI
            services.AddSingleton<AccountResolver>()
                .AddSingleton<IDocumentExecuter, DocumentExecuter>()
                .AddSingleton<RegisterInputType>()
                .AddSingleton<SigninInputType>()
                .AddSingleton<AccountMutation>()
                .AddSingleton<AccountQuery>()
                .AddSingleton<ListGraphType>()
                .AddSingleton<RegisterResultType>()
                .AddSingleton<SigninResultType>();

            var sp = services.BuildServiceProvider();

            services.AddSingleton<ISchema>(new AccountSchema(new FuncDependencyResolver(type => sp.GetService(type))));
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Config UseCors
            app.UseCors(MyAllowSpecificOrigins)
                .UseAuthentication()
                .UseHttpsRedirection()
                .UseGraphiQl("/api/graphql")
                .UseMvc();
        }
    }
}
