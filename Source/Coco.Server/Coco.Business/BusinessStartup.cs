using Coco.Business.Contracts;
using Coco.Business.Implementation;
using Coco.Contract;
using Coco.DAL;
using Coco.DAL.Implementations;
using Coco.Entities.Domain.Identity;
using Coco.Entities.Domain.Auth;
using Coco.Entities.Domain.Dbo;
using Coco.Entities.Domain.Agri;
using Coco.IdentityDAL;
using Coco.IdentityDAL.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Coco.Business.ValidationStrategies;
using Coco.Business.Implementation.UserBusiness;
using Coco.Entities.Domain.Content;
using AutoMapper;
using Coco.Business.MappingProfiles;

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
                .AddTransient<IRoleBusiness, RoleBusiness>()
                .AddTransient<IUserAttributeBusiness, UserAttributeBusiness>()
                .AddTransient<IArticleCategoryBusiness, ArticleCategoryBusiness>();

            services.AddTransient<IRepository<User>, EfIdentityRepository<User>>()
                .AddTransient<IRepository<UserInfo>, EfIdentityRepository<UserInfo>>()
                .AddTransient<IRepository<Country>, EfIdentityRepository<Country>>()
                .AddTransient<IRepository<Role>, EfIdentityRepository<Role>>()
                .AddTransient<IRepository<UserPhoto>, EfIdentityRepository<UserPhoto>>()
                .AddTransient<IRepository<UserAttribute>, EfIdentityRepository<UserAttribute>>();

            services.AddTransient<IRepository<Product>, EfRepository<Product>>()
                .AddTransient<IRepository<ArticleCategory>, EfRepository<ArticleCategory>>()
                .AddTransient<ValidationStrategyContext>();

            _dalStartup.RegiserTypes(services);
            _userDalStartup.RegiserTypes(services);
        }
    }
}
