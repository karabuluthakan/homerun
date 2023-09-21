using System.ComponentModel;

namespace Core.ResponseContract;

public enum ResponseReason
{
    [Description("OK")] Ok = 200,
    [Description("CREATED")] Created = 201,
    [Description("NO_CONTENT")] NoContent = 204,
    [Description("INVALID_ARGUMENT")] InvalidArgument = 400,
    [Description("FAILED_PRECONDITION")] FailedPrecondition = InvalidArgument,
    [Description("OUT_OF_RANGE")] OutOfRange = InvalidArgument,
    [Description("UNAUTHENTICATED")] Unauthenticated = 401,
    [Description("PERMISSION_DENIED")] PermissionDenied = 403,
    [Description("NOT_FOUND")] NotFound = 404,
    [Description("ABORTED")] Aborted = 409,
    [Description("ALREADY_EXISTS")] AlreadyExists = Aborted,
    [Description("RESOURCE_EXHAUSTED")] ResourceExhausted = 429,
    [Description("CANCELLED")] Cancelled = 499,
    [Description("DATA_LOSS")] DataLoss = 500,
    [Description("UNKNOWN")] Unknown = DataLoss,
    [Description("INTERNAL")] Internal = DataLoss,
    [Description("NOT_IMPLEMENTED")] NotImplemented = 501,
    [Description("N/A")] NetworkError = 502,
    [Description("UNAVAILABLE")] Unavailable = 503,
    [Description("DEADLINE_EXCEEDED")] DeadlineExceeded = 504
}