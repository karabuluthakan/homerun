using System.ComponentModel;

namespace Core.Extensions;

public static class EnumExtensions
{
    public static string? GetDescription<T>(this T enumValue) where T : struct, IConvertible
    {
        if (!typeof(T).IsEnum) return default;

        var description = enumValue.ToString();
        var fieldInfo = enumValue.GetType().GetField(enumValue.ToString() ?? string.Empty);

        if (fieldInfo is null) return description;
        var attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
        if (attrs.Length > 0) description = ((DescriptionAttribute)attrs[0]).Description;

        return description;
    }
}