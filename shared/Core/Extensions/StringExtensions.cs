using System.Text.Json;
using System.Text.Json.Serialization;

namespace Core.Extensions;

public static class StringExtensions
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    public static TDest? FromJson<TDest>(this string? source)
    {
        return string.IsNullOrWhiteSpace(source)
            ? default
            : JsonSerializer.Deserialize<TDest>(source, SerializerOptions);
    }
}