using Coco.Business.Dtos.General;
using Coco.Core.Models;
using System.Collections.Generic;

namespace  Module.Api.Auth.Resolvers.Contracts
{
    public interface IGenderResolver
    {
        IEnumerable<ISelectOption> GetSelections();
    }
}
