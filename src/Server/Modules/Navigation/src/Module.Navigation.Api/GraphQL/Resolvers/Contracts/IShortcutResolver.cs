using Module.Navigation.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Module.Navigation.Api.GraphQL.Resolvers.Contracts
{
    public interface IShortcutResolver
    {
        Task<IList<ShortcutModel>> GetShortcutsAsync(ShortcutFilterModel criterias);
    }
}
