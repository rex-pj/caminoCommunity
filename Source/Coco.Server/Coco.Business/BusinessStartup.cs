using Coco.Business.Contracts;
using Coco.Business.Implementation;
using Coco.Contract;
using Coco.DAL;
using Coco.DAL.Implementations;
using Coco.Entities.Domain.Account;
using Coco.Entities.Domain.Auth;
using Coco.Entities.Domain.Farm;
using Coco.UserDAL;
using Coco.UserDAL.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            _userDalStartup = new UserDalStartup(_config);
        }

        public void RegiserTypes(IServiceCollection services)
        {
            services.AddTransient<IAccountBusiness, AccountBusiness>();
            services.AddTransient<IRoleBusiness, RoleBusiness>();
            services.AddTransient<IRepository<User>, EfUserRepository<User>>();
            services.AddTransient<IRepository<UserInfo>, EfUserRepository<UserInfo>>();
            services.AddTransient<IRepository<Role>, EfUserRepository<Role>>();
            services.AddTransient<IRepository<Product>, EfRepository<Product>>();

            _dalStartup.RegiserTypes(services);
            _userDalStartup.RegiserTypes(services);
        }
    }
}
