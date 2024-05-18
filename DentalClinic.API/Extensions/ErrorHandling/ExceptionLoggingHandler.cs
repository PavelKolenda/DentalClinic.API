using Microsoft.AspNetCore.Diagnostics;

namespace DentalClinic.API.Extensions.ErrorHandling;

public class ExceptionLoggingHandler : IExceptionHandler
{
    private readonly ILogger<ExceptionLoggingHandler> _logger;

    public ExceptionLoggingHandler(ILogger<ExceptionLoggingHandler> logger)
    {
        _logger = logger;
    }

    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var exceptionMessage = exception.Message;

        _logger.LogError(
           "Message with TraceId:{traceId} failed with message:{exceptionMessage}",
           httpContext.TraceIdentifier, exceptionMessage
           );

        return ValueTask.FromResult(false);
    }
}