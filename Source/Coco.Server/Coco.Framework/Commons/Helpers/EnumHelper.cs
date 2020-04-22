using Coco.Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Coco.Framework.Commons.Helpers
{
    public class EnumHelper
    {
        public static IEnumerable<SelectOption> EnumToSelectList<TEnum>(string selectedId = "")
        {
            var enumsData = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();

            var enumType = (typeof(TEnum));
            var result = enumsData.Select(e => new SelectOption() {
                Id = Convert.ChangeType(e, typeof(int)).ToString(),
                Text = e.ToString()
            });

            return result;
        }
    }
}
