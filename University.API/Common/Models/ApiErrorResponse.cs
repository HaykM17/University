namespace University.API.Common.Models;

public class ApiErrorResponse
{
    public string Header { set; get; } = default!;
    public string Message { get; set; } = default!;
    public int StatusCode { get; set; }
    public string? LocalizedMessage { get; set; }
    public IDictionary<string, string[]>? Errors { get; set; }

    public static ApiErrorResponse FromValidation(Exception ex, IDictionary<string, string[]> errors)
    {
        return new ApiErrorResponse
        {
            Header = "Validation Failed",
            StatusCode = StatusCodes.Status400BadRequest,
            Errors = errors
        };
    }

    public static ApiErrorResponse FromException(Exception ex, int statusCode = StatusCodes.Status500InternalServerError)
    {
        return new ApiErrorResponse
        {
            Header = "Internal server error",
            Message = ex.Message,
            StatusCode = statusCode,
            Errors = null
        };
    }
}