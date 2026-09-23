
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public static class ApplicationLogsService
{

public static async Task<ApiResult<List<ApplicationLogsViewDTO>>> GetAllApplicationLogsAsync(
    CancellationToken cancellationToken = default)
{
    var result = await ApplicationLogsData.GetAllApplicationLogsAsync(cancellationToken);

    if (result.Data == null || result.Data.Count == 0)
    {
        return new ApiResult<List<ApplicationLogsViewDTO>>(
            null,
            "No ApplicationLogss found.",
            ErrorType.NotFound
        );
    }

    return new ApiResult<List<ApplicationLogsViewDTO>>(
        result.Data,
        "ApplicationLogss retrieved successfully.",
        ErrorType.None
    );
}


public static async Task<ApiResult<ApplicationLogsDTO>> AddApplicationLogsAsync(
    ApplicationLogsDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null)
    {
        return new ApiResult<ApplicationLogsDTO>
        {
            Data = null,
            Message = "ApplicationLogs cannot be null.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await ApplicationLogsData.AddApplicationLogsAsync(dto, cancellationToken);
}


public static async Task<ApiResult<ApplicationLogsViewDTO>> GetApplicationLogsByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
  
    return await ApplicationLogsData.GetApplicationLogsByIDAsync(
        ID,
        cancellationToken);
}

}

