using System.Text.Json;
using System.Text.Json.Serialization;
using Core.Extensions;

namespace Core.ResponseContract.Converters;

public class ResponseReasonJsonConverter : JsonConverter<ResponseReason>
{
    public override ResponseReason Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return Enum.Parse<ResponseReason>(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, ResponseReason value, JsonSerializerOptions options)
    {
        var description = value.GetDescription();
        writer.WriteStringValue(description);
    }
}