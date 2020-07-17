using Coco.Core.Dtos.General;
using System.Collections.Generic;

namespace  Coco.Api.Auth.Resolvers.Contracts
{
    public interface IGenderResolver
    {
        IEnumerable<ISelectOption> GetSelections();
    }
}
