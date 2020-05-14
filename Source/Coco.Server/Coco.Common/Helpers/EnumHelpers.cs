using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Coco.Common.Helpers
{
    public static class EnumHelpers
    {
        public static IEnumerable<SelectListItem> ToSelectListItems<TEnum>() where TEnum : struct, IConvertible, IFormattable
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().Select(x => new SelectListItem
            {
                Value = x.ToString("d", null),
                Text = x.ToString()
            });
        }

        public static IEnumerable<SelectListItem> ToSelectListItems<TEnum>(TEnum selected) where TEnum : struct, IConvertible, IFormattable
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().Select(x => new SelectListItem
            {
                Selected = x.Equals(selected),
                Value = x.ToString("d", null),
                Text = x.ToString()
            });
        }

        public static TEnum FilterEnumByName<TEnum>(string filter) where TEnum : struct, IConvertible, IFormattable
        {
            var data = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
            foreach(var item in data)
            {
                if (filter.StartsWith(item.ToString()))
                {
                    return item;
                }
            }

            return default;
        }
    }
}
