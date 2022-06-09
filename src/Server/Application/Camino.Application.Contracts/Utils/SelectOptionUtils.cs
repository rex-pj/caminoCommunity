using System.ComponentModel;

namespace Camino.Application.Contracts.Utils
{
    public static class SelectOptionUtils
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
    }
}
