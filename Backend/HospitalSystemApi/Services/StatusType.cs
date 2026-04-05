
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public static class StatusTypeService
{

public static async Task<ApiResult<List<StatusTypeViewDTO>>> GetAllStatusTypeAsync(
    CancellationToken cancellationToken = default)
{
    var result = await StatusTypeData.GetAllStatusTypeAsync(cancellationToken);

    if (result.Data == null || result.Data.Count == 0)
    {
        return new ApiResult<List<StatusTypeViewDTO>>(
            null,
            "No StatusTypes found.",
            ErrorType.NotFound
        );
    }

    return new ApiResult<List<StatusTypeViewDTO>>(
        result.Data,
        "StatusTypes retrieved successfully.",
        ErrorType.None
    );
}



public static async Task<ApiResult<StatusTypeViewDTO>> GetStatusTypeByStatusTypeIDAsync(
    int StatusTypeID,
    CancellationToken cancellationToken = default)
{
  
    return await StatusTypeData.GetStatusTypeByStatusTypeIDAsync(
        StatusTypeID,
        cancellationToken);
}


public static async Task<ApiResult<StatusTypeDTO>> AddStatusTypeAsync(
    StatusTypeDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null)
    {
        return new ApiResult<StatusTypeDTO>
        {
            Data = null,
            Message = "StatusType cannot be null.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await StatusTypeData.AddStatusTypeAsync(dto, cancellationToken);
}


public static async Task<ApiResult<StatusTypeUpdateDTO>> UpdateStatusTypeByStatusTypeIDAsync(
    StatusTypeUpdateDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null )
    {
        return new ApiResult<StatusTypeUpdateDTO>
        {
            Data = null,
            Message = "Invalid StatusType data.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await StatusTypeData.UpdateStatusTypeByStatusTypeIDAsync(dto, cancellationToken);
}


public static async Task<ApiResult<StatusTypeDTO>> DeleteStatusTypeByStatusTypeIDAsync(
    int StatusTypeID,
    CancellationToken cancellationToken = default)
{
   

    return await StatusTypeData.DeleteStatusTypeByStatusTypeIDAsync(
        StatusTypeID,
        cancellationToken);
}

}

