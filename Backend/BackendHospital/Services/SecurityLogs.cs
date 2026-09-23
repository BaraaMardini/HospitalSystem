
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public static class SecurityLogsService
{

public static async Task<ApiResult<List<SecurityLogsViewDTO>>> GetAllSecurityLogsAsync(
    CancellationToken cancellationToken = default)
{
    var result = await SecurityLogsData.GetAllSecurityLogsAsync(cancellationToken);

    if (result.Data == null || result.Data.Count == 0)
    {
        return new ApiResult<List<SecurityLogsViewDTO>>(
            null,
            "No SecurityLogss found.",
            ErrorType.NotFound
        );
    }

    return new ApiResult<List<SecurityLogsViewDTO>>(
        result.Data,
        "SecurityLogss retrieved successfully.",
        ErrorType.None
    );
}


public static async Task<ApiResult<SecurityLogsDTO>> AddSecurityLogsAsync(
    SecurityLogsDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null)
    {
        return new ApiResult<SecurityLogsDTO>
        {
            Data = null,
            Message = "SecurityLogs cannot be null.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await SecurityLogsData.AddSecurityLogsAsync(dto, cancellationToken);
}


public static async Task<ApiResult<SecurityLogsViewDTO>> GetSecurityLogsByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
  
    return await SecurityLogsData.GetSecurityLogsByIDAsync(
        ID,
        cancellationToken);
}

}

