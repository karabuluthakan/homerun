using Core.Extensions;
using Core.ResponseContract;
using Core.ResponseContract.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Api.Extensions;

public static class ControllerExtensions
{
    public static IActionResult ToResponse(this ControllerBase controller, IResponse response)
    {
        if (response.Success)
            switch (response.Reason)
            {
                case ResponseReason.NoContent: return controller.NoContent();
                case ResponseReason.Created:
                {
                    var createdResponse = response as CreatedResponse;
                    createdResponse!.Extensions.TryGetValue("id", out var id);
                    var uri = new Uri(id!.ToString()!, UriKind.Relative);
                    return controller.Created(uri, createdResponse);
                }
                default: return controller.Ok(response);
            }

        var request = controller.HttpContext.Request;
        var method = request.Method.ToUpperInvariant();
        var apiDefinition = request.Host.ToUriComponent()
            .Split("/")
            .LastOrDefault()!
            .ToUpperInvariant();

        var statusCode = (int)response.Reason;
        return controller.Problem(
            response.Detail,
            statusCode: statusCode,
            instance: $"{method}_{apiDefinition}",
            title: response.Reason.GetDescription(),
            type: $"https://httpstatuses.com/{statusCode}");
    }
}