using System;
using System.Linq;
using System.Reflection;

namespace Common.Utils
{
    public static class EnumHelper
    {
        /// <summary>
        /// Gets an attribute on an enum field value
        /// </summary>
        /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
        /// <param name="enumVal">The enum value</param>
        /// <returns>The attribute of type T that exists on the enum value</returns>
        /// <example>string desc = myEnumVariable.GetAttributeOfType<DescriptionAttribute>().Description;</example>
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : System.Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (T)attributes.FirstOrDefault();
        }

        public static T TryGetValueFromDescriptionWithDefault<T>(string description)
        {
            try
            {
                var type = typeof(T);
                if (!type.GetTypeInfo().IsEnum) throw new InvalidOperationException();
                foreach (var field in type.GetFields())
                {
                    var attribute = field.CustomAttributes.FirstOrDefault();

                    if (attribute != null)
                    {
                        var descAttr = attribute.ConstructorArguments.FirstOrDefault();
                        if (descAttr != null && descAttr.Value.ToString().Equals(description, StringComparison.CurrentCultureIgnoreCase))
                            return (T)field.GetValue(null);
                    }

                }
                return default(T);
            }
            catch
            {
                return default(T);
            }
        }
    }
}
