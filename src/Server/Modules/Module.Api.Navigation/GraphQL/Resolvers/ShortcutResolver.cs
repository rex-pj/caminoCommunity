using Camino.Infrastructure.GraphQL.Resolvers;
using System;
using System.Threading.Tasks;
using Module.Api.Navigation.GraphQL.Resolvers.Contracts;
using Module.Api.Navigation.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Camino.Application.Contracts.AppServices.Navigations;
using Camino.Shared.Configuration.Options;
using Camino.Application.Contracts.AppServices.Navigations.Dtos;
using Camino.Shared.Enums;

namespace Module.Api.Navigation.GraphQL.Resolvers
{
    public class ShortcutResolver : BaseResolver, IShortcutResolver
    {
        private readonly IShortcutAppService _shortcutAppService;
        private readonly PagerOptions _pagerOptions;

        public ShortcutResolver(IShortcutAppService shortcutAppService, IOptions<PagerOptions> pagerOptions)
            : base()
        {
            _shortcutAppService = shortcutAppService;
            _pagerOptions = pagerOptions.Value;
        }

        public async Task<IList<ShortcutModel>> GetShortcutsAsync(ShortcutFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new ShortcutFilterModel();
            }

            try
            {
                var shortcutPageList = await _shortcutAppService.GetAsync(new ShortcutFilter()
                {
                    Page = criterias.Page,
                    PageSize = _pagerOptions.PageSize,
                    Keyword = criterias.Search,
                    TypeId = criterias.TypeId
                });
                var shortcuts = shortcutPageList.Collections.Select(x => new ShortcutModel
                {
                    Description = x.Description,
                    Name = x.Name,
                    Icon = x.Icon,
                    Id = x.Id,
                    TypeId = (ShortcutTypes)x.TypeId,
                    Url = x.Url
                }).ToList();

                return shortcuts;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
