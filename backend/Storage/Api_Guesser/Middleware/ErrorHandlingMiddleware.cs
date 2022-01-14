using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared_Guesser;
using System;
using System.Threading.Tasks;

namespace Api_Guesser.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ILogger<ErrorHandlingMiddleware> logger)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, logger, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, ILogger<ErrorHandlingMiddleware> logger, Exception exception)
        {
            var exceptionId = Math.Abs(Guid.NewGuid().GetHashCode());
            var details = new ExtProblemDetails { Instance = context.Request.Path };

            switch (exception)
            {
                case BadRequestException ex:
                    details.Title = $"Bad request error #{exceptionId}";
                    details.Detail = ex.Message;
                    details.Status = StatusCodes.Status400BadRequest;
                    context.Response.StatusCode = 400;
                    break;
                case DuplicateEntityException ex:
                    details.Title = $"Duplicate entity error #{exceptionId}";
                    details.Detail = ex.Message;
                    details.Status = StatusCodes.Status409Conflict;
                    context.Response.StatusCode = 409;
                    break;
                case AccessDeniedException ex:
                    details.Title = $"Access denied error #{exceptionId}";
                    details.Detail = ex.Message;
                    details.Status = StatusCodes.Status401Unauthorized;
                    context.Response.StatusCode = 401;
                    break;
                case PaymentRequiredException ex:
                    details.Title = $"Payment required error #{exceptionId}";
                    details.Detail = ex.Message;
                    details.Status = StatusCodes.Status402PaymentRequired;
                    context.Response.StatusCode = 402;
                    break;
                case NoRightsException ex:
                    details.Title = $"No rights error #{exceptionId}";
                    details.Detail = ex.Message;
                    details.Status = StatusCodes.Status403Forbidden;
                    context.Response.StatusCode = 403;
                    break;
                case NotFoundException ex:
                    details.Title = $"Not found error #{exceptionId}";
                    details.Detail = ex.Message;
                    details.Status = StatusCodes.Status404NotFound;
                    context.Response.StatusCode = 404;
                    break;
                case UnacceptableStepException ex:
                    details.Title = $"Invalid table step #{exceptionId}";
                    details.Detail = ex.Message;
                    details.Status = StatusCodes.Status403Forbidden;
                    context.Response.StatusCode = 403;
                    break;
                default:
                    details.Title = $"Server error #{exceptionId}";
                    details.Detail = $"Contact administrator for error details";
                    details.Status = StatusCodes.Status500InternalServerError;
                    context.Response.StatusCode = 500;
                    break;
            }

            logger.LogError(exceptionId, exception, $"{details.Title}: {exception.Message}");

            context.Response.ContentType = "application/json";
            var result = System.Text.Json.JsonSerializer.Serialize(details);

            return context.Response.WriteAsync(result);
        }
    }
}