using Coco.Business.Contracts;
using Coco.Business.Implementation;
using Coco.Contract;
using Coco.DAL;
using Coco.DAL.Implementations;
using Coco.Entities.Domain.Identity;
using Coco.Entities.Domain.Auth;
using Coco.Entities.Domain.Dbo;
using Coco.Entities.Domain.Farm;
using Coco.IdentityDAL;
using Coco.IdentityDAL.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Coco.Business.ValidationStrategies;
using Coco.Business.Implementation.UserBusiness;
using AutoMapper;
using Coco.Entities.Model.User;
using System.Reflection;
using Coco.Business.Mapping;

namespace Coco.Business
{
    public class BusinessStartup : IBootstrapper
    {
        private readonly IConfiguration _config;
        private readonly IBootstrapper _dalStartup;
        private readonly IBootstrapper _userDalStartup;
        public BusinessStartup(IConfiguration config)
        {
            _config = config;
            _dalStartup = new DalStartup(_config);
            _userDalStartup = new IdentityDalStartup(_config);
        }

        public void RegiserTypes(IServiceCollection services)
        {
            services.AddTransient<IUserBusiness, UserBusiness>()
                .AddTransient<ICountryBusiness, CountryBusiness>()
                .AddTransient<IUserPhotoBusiness, UserPhotoBusiness>()
                .AddTransient<IRoleBusiness, RoleBusiness>();

            services.AddTransient<IRepository<User>, EfUserRepository<User>>()
                .AddTransient<IRepository<UserInfo>, EfUserRepository<UserInfo>>()
                .AddTransient<IRepository<Country>, EfUserRepository<Country>>()
                .AddTransient<IRepository<Role>, EfUserRepository<Role>>()
                .AddTransient<IRepository<UserPhoto>, EfUserRepository<UserPhoto>>();

            services.AddTransient<IRepository<Product>, EfRepository<Product>>()
                .AddTransient<ValidationStrategyContext>();

            services.AddAutoMapper(Assembly.GetAssembly(typeof(UserMappingProfile)));

            _dalStartup.RegiserTypes(services);
            _userDalStartup.RegiserTypes(services);
        }
    }
}
