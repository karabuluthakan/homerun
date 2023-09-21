using Core.ResponseContract.Abstract;

namespace Core.ResponseContract;

public class ErrorResponse : ResponseBase
{
    private ErrorResponse(ResponseReason reason, string detail, string instance) : base(reason, detail, instance)
    {
    }

    public static ErrorResponse InvalidArgument(string instance, string detail = "INVALID_ARGUMENT")
    {
        return new ErrorResponse(ResponseReason.InvalidArgument, detail, instance);
    }

    public static ErrorResponse FailedPrecondition(string instance, string detail = "FAILED_PRECONDITION")
    {
        return new ErrorResponse(ResponseReason.FailedPrecondition, detail, instance);
    }

    public static ErrorResponse OutOfRange(string instance, string detail = "RESOURCE_OUT_OF_RANGE")
    {
        return new ErrorResponse(ResponseReason.OutOfRange, detail, instance);
    }

    public static ErrorResponse Unauthenticated(string instance, string detail = "UNAUTHENTICATED")
    {
        return new ErrorResponse(ResponseReason.Unauthenticated, detail, instance);
    }

    public static ErrorResponse PermissionDenied(string instance, string detail = "PERMISSION_DENIED")
    {
        return new ErrorResponse(ResponseReason.PermissionDenied, detail, instance);
    }

    public static ErrorResponse NotFound(string instance, string detail = "RESOURCE_NOT_FOUND")
    {
        return new ErrorResponse(ResponseReason.NotFound, detail, instance);
    }

    public static ErrorResponse Aborted(string instance, string detail = "ABORTED")
    {
        return new ErrorResponse(ResponseReason.Aborted, detail, instance);
    }

    public static ErrorResponse AlreadyExists(string instance, string detail = "RESOURCE_ALREADY_EXISTS")
    {
        return new ErrorResponse(ResponseReason.AlreadyExists, detail, instance);
    }

    public static ErrorResponse ResourceExhausted(string instance, string detail = "RESOURCE_EXHAUSTED")
    {
        return new ErrorResponse(ResponseReason.ResourceExhausted, detail, instance);
    }

    public static ErrorResponse Cancelled(string instance, string detail = "CANCELLED")
    {
        return new ErrorResponse(ResponseReason.Cancelled, detail, instance);
    }

    public static ErrorResponse DataLoss(string instance, string detail = "DATA_LOSS")
    {
        return new ErrorResponse(ResponseReason.DataLoss, detail, instance);
    }

    public static ErrorResponse Unknown(string instance, string detail = "UNKNOWN")
    {
        return new ErrorResponse(ResponseReason.Unknown, detail, instance);
    }

    public static ErrorResponse Internal(string instance, string detail = "INTERNAL")
    {
        return new ErrorResponse(ResponseReason.Internal, detail, instance);
    }

    public static ErrorResponse NotImplemented(string instance, string detail = "NOT_IMPLEMENTED")
    {
        return new ErrorResponse(ResponseReason.NotImplemented, detail, instance);
    }

    public static ErrorResponse NetworkError(string instance, string detail = "N/A")
    {
        return new ErrorResponse(ResponseReason.NetworkError, detail, instance);
    }

    public static ErrorResponse Unavailable(string instance, string detail = "UNAVAILABLE")
    {
        return new ErrorResponse(ResponseReason.Unavailable, detail, instance);
    }

    public static ErrorResponse DeadlineExceeded(string instance, string detail = "DEADLINE_EXCEEDED")
    {
        return new ErrorResponse(ResponseReason.DeadlineExceeded, detail, instance);
    }
}