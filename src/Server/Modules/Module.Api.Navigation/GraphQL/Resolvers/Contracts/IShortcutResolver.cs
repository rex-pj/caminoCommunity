using Module.Api.Navigation.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Module.Api.Navigation.GraphQL.Resolvers.Contracts
{
    public interface IShortcutResolver
    {
        Task<IList<ShortcutModel>> GetShortcutsAsync(ShortcutFilterModel criterias);
    }
}
