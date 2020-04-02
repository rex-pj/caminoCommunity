using Api.Public.Resolvers.Contracts;
using Coco.Api.Framework.Commons.Helpers;
using Coco.Api.Framework.Models;
using Coco.Entities.Enums;
using System.Collections.Generic;

namespace Api.Public.Resolvers
{
    public class GenderResolver : IGenderResolver
    {
        public IEnumerable<ISelectOption> GetSelections()
        {
            return EnumHelper.EnumToSelectList<GenderEnum>();
        }
    }
}
