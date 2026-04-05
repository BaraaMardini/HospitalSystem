
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public static class Work_DayService
{

public static async Task<ApiResult<List<Work_DayViewDTO>>> GetAllWork_DayAsync(
    CancellationToken cancellationToken = default)
{
    var result = await Work_DayData.GetAllWork_DayAsync(cancellationToken);

    if (result.Data == null || result.Data.Count == 0)
    {
        return new ApiResult<List<Work_DayViewDTO>>(
            null,
            "No Work_Days found.",
            ErrorType.NotFound
        );
    }

    return new ApiResult<List<Work_DayViewDTO>>(
        result.Data,
        "Work_Days retrieved successfully.",
        ErrorType.None
    );
}



public static async Task<ApiResult<Work_DayViewDTO>> GetWork_DayByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
  
    return await Work_DayData.GetWork_DayByIDAsync(
        ID,
        cancellationToken);
}


public static async Task<ApiResult<Work_DayDTO>> AddWork_DayAsync(
    Work_DayDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null)
    {
        return new ApiResult<Work_DayDTO>
        {
            Data = null,
            Message = "Work_Day cannot be null.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await Work_DayData.AddWork_DayAsync(dto, cancellationToken);
}


public static async Task<ApiResult<Work_DayUpdateDTO>> UpdateWork_DayByIDAsync(
    Work_DayUpdateDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null )
    {
        return new ApiResult<Work_DayUpdateDTO>
        {
            Data = null,
            Message = "Invalid Work_Day data.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await Work_DayData.UpdateWork_DayByIDAsync(dto, cancellationToken);
}


public static async Task<ApiResult<Work_DayDTO>> DeleteWork_DayByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
   

    return await Work_DayData.DeleteWork_DayByIDAsync(
        ID,
        cancellationToken);
}

}

