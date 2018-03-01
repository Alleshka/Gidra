using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Reflection;

namespace GidraSIM.Core.Model
{
    public static class EnumExtension
    {
        public static string Description<T>(this T value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            var attributes = field.GetCustomAttributes(false);
            if (attributes.Any())
            {
                return (attributes.ElementAt(0) as DescriptionAttribute)?.Description;
            }
            return " ";
        }

        public static T GetEnum<T>(this string value)
        {
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                T en = (T)Enum.Parse(typeof(T), item.ToString());
                if (String.CompareOrdinal(value, (en).Description()) == 0)
                    return en;
            }
            return default(T);
        }
    }
}