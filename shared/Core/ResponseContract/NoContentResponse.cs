using Core.ResponseContract.Abstract;

namespace Core.ResponseContract;

public sealed class NoContentResponse : ResponseBase
{
    private NoContentResponse(string detail, string instance) : base(ResponseReason.NoContent, detail, instance)
    {
    }

    public static NoContentResponse Successful(string instance, string detail = "RESOURCE_SUCCESSFULLY_MODIFIED")
    {
        return new NoContentResponse(detail, instance);
    }
}