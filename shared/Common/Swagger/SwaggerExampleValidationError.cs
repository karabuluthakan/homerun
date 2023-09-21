using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Common.Swagger;

public sealed class SwaggerExampleValidationError : IExamplesProvider<ProblemDetails>
{
    public ProblemDetails GetExamples()
    {
        List<ValidationFailure> failures = new()
        {
            new ValidationFailure("property1", "Not null!"),
            new ValidationFailure("property1", "Not empty!"),
            new ValidationFailure("property2", "Not null!"),
            new ValidationFailure("property2", "Not empty!")
        };
        var validationException = new ValidationException(failures);
        var problem = new ProblemDetails
        {
            Title = "VALIDATION_ERROR",
            Status = StatusCodes.Status400BadRequest,
            Type = "https://httpstatuses.com/400",
            Detail = "One or more validation errors occurred."
        };

        Dictionary<string, string[]> errors = new();
        foreach (var failure in validationException.Errors)
        {
            var key = failure.PropertyName;
            var value = failure.ErrorMessage;
            if (errors.ContainsKey(key))
            {
                var array = errors[key].Append(value).ToArray();
                errors[key] = array;
                continue;
            }

            errors.Add(key, new[] { value });
        }

        problem.Extensions.Add("errors", errors);

        return problem;
    }
}