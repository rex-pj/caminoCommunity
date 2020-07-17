using Coco.Api.Auth.Resolvers.Contracts;
using Coco.Core.Entities.Enums;
using System.Collections.Generic;
using Coco.Core.Utils;
using Coco.Core.Dtos.General;

namespace  Coco.Api.Auth.Resolvers
{
    public class GenderResolver : IGenderResolver
    {
        public IEnumerable<ISelectOption> GetSelections()
        {
            return EnumUtil.EnumToSelectList<Gender>();
        }
    }
}
