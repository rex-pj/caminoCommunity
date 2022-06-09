using System.ComponentModel;

namespace Camino.Shared.Utils
{
    public static class EnumUtil
    {
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

        public static int GetCode<TEnum>(this TEnum value) where TEnum : struct, IConvertible, IFormattable
        {
            return Convert.ToInt32(value);
        }
    }
}
