using Camino.Framework.GraphQL.Resolvers;
using Camino.Shared.Requests.Filters;
using System;
using System.Threading.Tasks;
using Module.Api.Navigation.GraphQL.Resolvers.Contracts;
using Camino.Core.Contracts.Services.Navigations;
using Module.Api.Navigation.Models;
using System.Collections.Generic;
using System.Linq;
using Camino.Shared.Configurations;
using Microsoft.Extensions.Options;
using Camino.Infrastructure.Commons.Enums;

namespace Module.Api.Navigation.GraphQL.Resolvers
{
    public class ShortcutResolver : BaseResolver, IShortcutResolver
    {
        private readonly IShortcutService _shortcutService;
        private readonly PagerOptions _pagerOptions;

        public ShortcutResolver(IShortcutService shortcutService, IOptions<PagerOptions> pagerOptions)
            : base()
        {
            _shortcutService = shortcutService;
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
                var shortcutPageList = await _shortcutService.GetAsync(new ShortcutFilter()
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
                    TypeId = (ShortcutType)x.TypeId,
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
