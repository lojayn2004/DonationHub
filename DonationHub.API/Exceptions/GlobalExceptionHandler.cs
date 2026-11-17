using DonationHub.Application.Common.Dtos;
using Microsoft.AspNetCore.Diagnostics;

namespace DonationHub.API.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(
           ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext context,
                                                    Exception exception, 
                                                    CancellationToken cancellationToken)
        {
            _logger.LogError(exception,
                     "Exception Ocuured: {Message}",
                     exception.Message);

            var problemDetails = new ProblemDetailsDto
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occurred",
                TraceId = context.TraceIdentifier,
                Code = "Server Error",
                Type = $"https://httpstatuses.io/500"

            };
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/problem+json";
            await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
