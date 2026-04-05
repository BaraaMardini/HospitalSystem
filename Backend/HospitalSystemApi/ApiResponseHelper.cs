using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

public static class ApiResponseHelper
{
    public static ActionResult GenerateApiResponse<T>(
        ControllerBase controller,
        ErrorType errorType,
        ApiResult<T>? result,
        int? newID = null,
        string? routeName = null,
        string? routeParamName = "id")
    {
        T? safeData = result != null ? result.Data : default;

        var response = new
        {
            data = safeData,
            message = result?.Message ?? GetDefaultMessage(errorType),
            errorCode = (int)errorType
        };

        return errorType switch
        {
            ErrorType.None =>
                (routeName != null && newID.HasValue)
                    ? controller.CreatedAtRoute(
                        routeName,
                        new RouteValueDictionary { { routeParamName!, newID.Value } },
                        response
                    )
                    : controller.Ok(response),

            ErrorType.InvalidId =>
                controller.BadRequest(response),

            ErrorType.NotFound =>
                controller.NotFound(response),

            ErrorType.AlreadyExists =>
                controller.Conflict(response),
            ErrorType.LogicalDependency =>
controller.Conflict(response),

            ErrorType.DatabaseError =>
                controller.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    response),

            _ =>
                controller.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { data = default(T), message = "Unexpected error occurred", errorCode = 500 })
        };
    }

    private static string GetDefaultMessage(ErrorType errorType) => errorType switch
    {
        ErrorType.None => "Operation successful",
        ErrorType.InvalidId => "Invalid ID provided",
        ErrorType.NotFound => "Resource not found",
        ErrorType.AlreadyExists => "Resource already exists",
        ErrorType.DatabaseError => "A database error occurred",
        _ => "Unexpected error occurred"
    };
}
