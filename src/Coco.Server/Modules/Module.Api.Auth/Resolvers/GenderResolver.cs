using Module.Api.Auth.Resolvers.Contracts;
using Coco.Data.Enums;
using System.Collections.Generic;
using Coco.Core.Utils;
using Coco.Core.Models;

namespace  Module.Api.Auth.Resolvers
{
    public class GenderResolver : IGenderResolver
    {
        public IEnumerable<ISelectOption> GetSelections()
        {
            return EnumUtil.EnumToSelectList<GenderType>();
        }
    }
}
