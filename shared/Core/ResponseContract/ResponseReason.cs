using System.ComponentModel;
using Core.Attributes;

namespace Core.ResponseContract;

public enum ResponseReason
{
    [SwaggerIgnoreEnum] [Description("OK")]
    Ok = 200,

    [SwaggerIgnoreEnum] [Description("CREATED")]
    Created = 201,

    [SwaggerIgnoreEnum] [Description("NO_CONTENT")]
    NoContent = 204,

    [SwaggerIgnoreEnum] [Description("INVALID_ARGUMENT")]
    InvalidArgument = 400,

    [SwaggerIgnoreEnum] [Description("FAILED_PRECONDITION")]
    FailedPrecondition = InvalidArgument,

    [SwaggerIgnoreEnum] [Description("OUT_OF_RANGE")]
    OutOfRange = InvalidArgument,

    [SwaggerIgnoreEnum] [Description("UNAUTHENTICATED")]
    Unauthenticated = 401,

    [SwaggerIgnoreEnum] [Description("PERMISSION_DENIED")]
    PermissionDenied = 403,

    [SwaggerIgnoreEnum] [Description("NOT_FOUND")]
    NotFound = 404,

    [SwaggerIgnoreEnum] [Description("ABORTED")]
    Aborted = 409,

    [SwaggerIgnoreEnum] [Description("ALREADY_EXISTS")]
    AlreadyExists = Aborted,

    [SwaggerIgnoreEnum] [Description("RESOURCE_EXHAUSTED")]
    ResourceExhausted = 429,

    [SwaggerIgnoreEnum] [Description("CANCELLED")]
    Cancelled = 499,

    [SwaggerIgnoreEnum] [Description("DATA_LOSS")]
    DataLoss = 500,

    [SwaggerIgnoreEnum] [Description("UNKNOWN")]
    Unknown = DataLoss,

    [SwaggerIgnoreEnum] [Description("INTERNAL")]
    Internal = DataLoss,

    [SwaggerIgnoreEnum] [Description("NOT_IMPLEMENTED")]
    NotImplemented = 501,

    [SwaggerIgnoreEnum] [Description("N/A")]
    NetworkError = 502,

    [SwaggerIgnoreEnum] [Description("UNAVAILABLE")]
    Unavailable = 503,

    [SwaggerIgnoreEnum] [Description("DEADLINE_EXCEEDED")]
    DeadlineExceeded = 504
}