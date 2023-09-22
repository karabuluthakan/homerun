using System.Text.Json.Serialization;
using Core.Attributes;
using Core.Extensions;
using Core.ResponseContract.Converters;

namespace Core.ResponseContract.Abstract;

public abstract class ResponseBase : IResponse
{
    [JsonConstructor]
    protected ResponseBase(ResponseReason reason, string detail, string instance)
    {
        var statusCode = (int)reason;
        Reason = reason;
        Success = reason < ResponseReason.InvalidArgument;
        Detail = string.IsNullOrWhiteSpace(detail) ? reason.GetDescription()! : detail.Trim();
        Status = statusCode;
        Type = $"https://httpstatuses.com/{statusCode}";
        Instance = instance.Trim().ToUpperInvariant();
    }

    [JsonPropertyName("type")]
    [JsonPropertyOrder(5)]
    public string Type { get; }

    [JsonPropertyName("status")]
    [JsonPropertyOrder(3)]
    public int Status { get; }

    [JsonPropertyName("instance")]
    [JsonPropertyOrder(1)]
    public string Instance { get; }

    [JsonPropertyName("title")]
    [JsonConverter(typeof(ResponseReasonJsonConverter))]
    [JsonPropertyOrder(4)]
    public ResponseReason Reason { get; }

    [JsonPropertyName("detail")]
    [JsonPropertyOrder(0)]
    public string Detail { get; }

    [JsonIgnore] public bool Success { get; }

    [JsonExtensionData]
    [JsonPropertyOrder(4)]
    public IDictionary<string, object> Extensions { get; } = new Dictionary<string, object>(StringComparer.Ordinal);
}