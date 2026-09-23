
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public static class StatusService
{

public static async Task<ApiResult<List<StatusViewDTO>>> GetAllStatusAsync(
    CancellationToken cancellationToken = default)
{
    var result = await StatusData.GetAllStatusAsync(cancellationToken);

    if (result.Data == null || result.Data.Count == 0)
    {
        return new ApiResult<List<StatusViewDTO>>(
            null,
            "No Statuss found.",
            ErrorType.NotFound
        );
    }

    return new ApiResult<List<StatusViewDTO>>(
        result.Data,
        "Statuss retrieved successfully.",
        ErrorType.None
    );
}


public static async Task<ApiResult<StatusDTO>> AddStatusAsync(
    StatusDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null)
    {
        return new ApiResult<StatusDTO>
        {
            Data = null,
            Message = "Status cannot be null.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await StatusData.AddStatusAsync(dto, cancellationToken);
}


public static async Task<ApiResult<StatusDTO>> UpdateStatusByIDAsync(
    StatusUpdateDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null )
    {
        return new ApiResult<StatusDTO>
        {
            Data = null,
            Message = "Invalid Status data.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await StatusData.UpdateStatusByIDAsync(dto, cancellationToken);
}


public static async Task<ApiResult<StatusDTO>> DeleteStatusByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
   

    return await StatusData.DeleteStatusByIDAsync(
        ID,
        cancellationToken);
}


public static async Task<ApiResult<List<StatusViewDTO>>> SearchStatus(
    string? StatusTypeCode,
    CancellationToken cancellationToken = default)
{
    var result = await StatusData.SearchStatus(
        StatusTypeCode,
        cancellationToken);

    if (result.Data == null || result.Data.Count == 0)
    {
        return new ApiResult<List<StatusViewDTO>>(
            null,
            "No Statuss found.",
            ErrorType.NotFound
        );
    }

    return new ApiResult<List<StatusViewDTO>>(
        result.Data,
        "Statuss retrieved successfully.",
        ErrorType.None
    );
}


public static async Task<ApiResult<StatusViewDTO>> GetStatusByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
  
    return await StatusData.GetStatusByIDAsync(
        ID,
        cancellationToken);
}

}

