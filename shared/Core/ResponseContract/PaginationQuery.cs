using System.Text.Json.Serialization;
using Core.ResponseContract.Abstract;

namespace Core.ResponseContract;

public sealed class PaginationQuery : IPaginationQuery
{
    private const int DefaultPageNumber = 1;
    private const int DefaultPageSize = 10;
    [JsonPropertyName("pageNumber")] public int PageNumber { get; }
    [JsonPropertyName("pageSize")] public int PageSize { get; }

    [JsonConstructor]
    public PaginationQuery(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber < DefaultPageNumber ? DefaultPageNumber : pageNumber;
        PageSize = pageSize < DefaultPageSize ? DefaultPageSize : pageSize;
    }

    [JsonConstructor]
    public PaginationQuery()
    {
        PageNumber = DefaultPageNumber;
        PageSize = DefaultPageSize;
    }
}