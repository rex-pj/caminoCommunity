using Coco.Framework.Models;
using System.Collections.Generic;

namespace Api.Auth.Resolvers.Contracts
{
    public interface IGenderResolver
    {
        IEnumerable<ISelectOption> GetSelections();
    }
}
