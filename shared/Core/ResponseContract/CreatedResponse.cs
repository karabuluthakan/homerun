using Core.ResponseContract.Abstract;

namespace Core.ResponseContract;

public sealed class CreatedResponse : ResponseBase
{
    private CreatedResponse(string detail, string instance) : base(ResponseReason.Created, detail, instance)
    {
    }

    public static CreatedResponse Successful(string id, string instance, string detail = "RESOURCE_SUCCESSFULLY_CREATED")
    {
        var response = new CreatedResponse(detail, instance);
        response.Extensions.TryAdd("id", id);
        return response;
    }
}