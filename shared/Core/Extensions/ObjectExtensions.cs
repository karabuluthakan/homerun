using System.Text.Json;
using System.Text.Json.Serialization;

namespace Core.Extensions;

public static class ObjectExtensions
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    public static string ToJson(this object? source)
    {
        return source is null ? string.Empty : JsonSerializer.Serialize(source, SerializerOptions);
    }

    public static string ToJson(this object? source,Type type)
    {
        return source is null ? string.Empty : JsonSerializer.Serialize(source,type, SerializerOptions);
    }
}