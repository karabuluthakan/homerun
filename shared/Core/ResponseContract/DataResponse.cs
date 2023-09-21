using Core.Domain.DataTransferObjects;
using Core.ResponseContract.Abstract;

namespace Core.ResponseContract;

public sealed class DataResponse : ResponseBase
{
    private const string Key = "data";

    private DataResponse(string detail, string instance) : base(ResponseReason.Ok, detail, instance)
    {
    }

    public static DataResponse Successful<T>(T data, string instance, string detail = "RESOURCE_SUCCESSFULLY_VIEWED")
        where T : class, IDto, new()
    {
        var response = new DataResponse(detail, instance);
        response.Extensions.Add(Key, data);
        return response;
    }

    public static DataResponse Successful<T>(List<T> data, string instance,
        string detail = "RESOURCE_SUCCESSFULLY_LISTED")
        where T : class, IDto, new()
    {
        var response = new DataResponse(detail, instance);
        response.Extensions.Add(Key, data);
        return response;
    }
}