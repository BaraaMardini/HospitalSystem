
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public static class Work_ScheduleService
{


    public static async Task<ApiResult<List<Work_ScheduleViewDTO>>> GetAllWork_ScheduleByDoctorIDAsync(
        int DoctorID,
        CancellationToken cancellationToken = default)
    {
        var result = await Work_ScheduleData.GetAllWork_ScheduleByDoctorIDAsync(
            DoctorID,
            cancellationToken);

        if (result.Data == null || result.Data.Count == 0)
        {
            return new ApiResult<List<Work_ScheduleViewDTO>>(
                null,
                "No Work_Schedules found.",
                ErrorType.NotFound
            );
        }

        return new ApiResult<List<Work_ScheduleViewDTO>>(
            result.Data,
            "Work_Schedules retrieved successfully.",
            ErrorType.None
        );
    }
    public static async Task<ApiResult<List<Work_ScheduleViewDTO>>> GetAllWork_ScheduleAsync(
    CancellationToken cancellationToken = default)
{
    var result = await Work_ScheduleData.GetAllWork_ScheduleAsync(cancellationToken);

    if (result.Data == null || result.Data.Count == 0)
    {
        return new ApiResult<List<Work_ScheduleViewDTO>>(
            null,
            "No Work_Schedules found.",
            ErrorType.NotFound
        );
    }

    return new ApiResult<List<Work_ScheduleViewDTO>>(
        result.Data,
        "Work_Schedules retrieved successfully.",
        ErrorType.None
    );
}



public static async Task<ApiResult<Work_ScheduleViewDTO>> GetWork_ScheduleByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
  
    return await Work_ScheduleData.GetWork_ScheduleByIDAsync(
        ID,
        cancellationToken);
}


public static async Task<ApiResult<Work_ScheduleDTO>> AddWork_ScheduleAsync(
    Work_ScheduleDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null)
    {
        return new ApiResult<Work_ScheduleDTO>
        {
            Data = null,
            Message = "Work_Schedule cannot be null.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await Work_ScheduleData.AddWork_ScheduleAsync(dto, cancellationToken);
}


public static async Task<ApiResult<Work_ScheduleUpdateDTO>> UpdateWork_ScheduleByIDAsync(
    Work_ScheduleUpdateDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null )
    {
        return new ApiResult<Work_ScheduleUpdateDTO>
        {
            Data = null,
            Message = "Invalid Work_Schedule data.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await Work_ScheduleData.UpdateWork_ScheduleByIDAsync(dto, cancellationToken);
}


public static async Task<ApiResult<Work_ScheduleDTO>> DeleteWork_ScheduleByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
   

    return await Work_ScheduleData.DeleteWork_ScheduleByIDAsync(
        ID,
        cancellationToken);
}

}

