using Api.Auth.Resolvers.Contracts;
using Coco.Entities.Enums;
using System.Collections.Generic;
using Coco.Entities.Models;
using Coco.Common.Helpers;

namespace Api.Auth.Resolvers
{
    public class GenderResolver : IGenderResolver
    {
        public IEnumerable<ISelectOption> GetSelections()
        {
            return EnumHelpers.EnumToSelectList<GenderEnum>();
        }
    }
}
