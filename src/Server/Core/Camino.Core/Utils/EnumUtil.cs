using Camino.Shared.General;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Camino.Core.Utils
{
    public static class EnumUtil
    {
        public static IEnumerable<SelectOption> ToSelectOptions<TEnum>() where TEnum : struct, IConvertible, IFormattable
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().Select(enumValue => new SelectOption()
            {
                Id = enumValue.ToString("d", null),
                Text = enumValue.GetEnumDescription()
            });
        }

        public static IEnumerable<SelectOption> ToSelectOptions<TEnum>(TEnum selected) where TEnum : struct, IConvertible, IFormattable
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().Select(enumValue => new SelectOption
            {
                IsSelected = enumValue.Equals(selected),
                Id = enumValue.ToString("d", null),
                Text = enumValue.GetEnumDescription()
            });
        }

        public static string GetEnumDescription<TEnum>(this TEnum value) where TEnum : struct, IConvertible, IFormattable
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var descriptionAttributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
            if (descriptionAttributes == null || !descriptionAttributes.Any())
            {
                return value.ToString();
            }

            return descriptionAttributes.First().Description;
        }

        public static TEnum FilterEnumByName<TEnum>(string filter) where TEnum : struct, IConvertible, IFormattable
        {
            var data = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
            foreach (var item in data)
            {
                if (filter.StartsWith(item.ToString()))
                {
                    return item;
                }
            }

            return default;
        }


        public static string GetDescription<TEnum>(TEnum value)
        {
            var type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                var field = type.GetField(name);
                if (field != null)
                {
                    var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attribute != null)
                    {
                        return attribute.Description;
                    }
                }
            }
            return value.ToString();
        }
    }
}
