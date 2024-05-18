using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DentalClinic.API.Extensions.ErrorHandling;
public class GlobalExceptionsHandling : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        httpContext.Response.WriteAsJsonAsync(new ProblemDetails()
        {
            Status = httpContext.Response.StatusCode,
            Detail = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
        }, cancellationToken);

        return ValueTask.FromResult(true);
    }
}