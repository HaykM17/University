using Application.Common.Exceptions;
using University.API.Common.Models;

namespace University.API.Common.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate _next, ILogger<ExceptionHandlingMiddleware> _logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            var response = new ApiErrorResponse
            {
                Message = ex.Message,
                StatusCode = StatusCodes.Status404NotFound,
            };

            context.Response.StatusCode = StatusCodes.Status404NotFound;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (RestApiException ex)
        {
            var response = new ApiErrorResponse
            {
                Message = ex.Message,
                StatusCode = (int)ex.StatusCode,
            };

            context.Response.StatusCode = (int)ex.StatusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            var response = ApiErrorResponse.FromException(ex);

            context.Response.StatusCode = response.StatusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(response);
        }

    }
}