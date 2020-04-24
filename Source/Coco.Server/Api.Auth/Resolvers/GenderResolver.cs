using Api.Auth.Resolvers.Contracts;
using Coco.Framework.Commons.Helpers;
using Coco.Framework.Models;
using Coco.Entities.Enums;
using System.Collections.Generic;

namespace Api.Auth.Resolvers
{
    public class GenderResolver : IGenderResolver
    {
        public IEnumerable<ISelectOption> GetSelections()
        {
            return EnumHelper.EnumToSelectList<GenderEnum>();
        }
    }
}
