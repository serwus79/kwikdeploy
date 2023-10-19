using Microsoft.AspNetCore.Mvc;

namespace Api.Common;

/// <summary>
///     A <see cref="HttpValidationProblemDetails" /> for custom errors.
///     Based on <see cref="ProblemDetails" />
/// </summary>
public class ProblemDetailsWithErrors : ProblemDetails
{
    public ProblemDetailsWithErrors(IDictionary<string, string>? errors)
    {
        if (errors is not null)
        {
            Errors = errors;
        }
    }

    public IDictionary<string, string> Errors { get; } = new Dictionary<string, string>(StringComparer.Ordinal);
}