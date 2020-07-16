using Api.Auth.Resolvers.Contracts;
using Coco.Core.Entities.Enums;
using System.Collections.Generic;
using Coco.Core.Helpers;
using Coco.Core.Dtos.General;

namespace Api.Auth.Resolvers
{
    public class GenderResolver : IGenderResolver
    {
        public IEnumerable<ISelectOption> GetSelections()
        {
            return EnumHelper.EnumToSelectList<Gender>();
        }
    }
}
