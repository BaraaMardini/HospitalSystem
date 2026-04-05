
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public static class MedicalHistoryService
{

public static async Task<ApiResult<List<MedicalHistoryViewDTO>>> GetAllMedicalHistoryAsync(
    CancellationToken cancellationToken = default)
{
    var result = await MedicalHistoryData.GetAllMedicalHistoryAsync(cancellationToken);

    if (result.Data == null || result.Data.Count == 0)
    {
        return new ApiResult<List<MedicalHistoryViewDTO>>(
            null,
            "No MedicalHistorys found.",
            ErrorType.NotFound
        );
    }

    return new ApiResult<List<MedicalHistoryViewDTO>>(
        result.Data,
        "MedicalHistorys retrieved successfully.",
        ErrorType.None
    );
}


    public static async Task<ApiResult<List<MedicalHistoryViewDTO>>> GetAllMedicalHistoryByNameAsync(
        string Name,
        CancellationToken cancellationToken = default)
    {
        var result = await MedicalHistoryData.GetAllMedicalHistoryByNameAsync(
            Name,
            cancellationToken);

        if (result.Data == null || result.Data.Count == 0)
        {
            return new ApiResult<List<MedicalHistoryViewDTO>>(
                null,
                "No MedicalHistorys found.",
                ErrorType.NotFound
            );
        }

        return new ApiResult<List<MedicalHistoryViewDTO>>(
            result.Data,
            "MedicalHistorys retrieved successfully.",
            ErrorType.None
        );
    }


    public static async Task<ApiResult<MedicalHistoryViewDTO>> GetMedicalHistoryByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
  
    return await MedicalHistoryData.GetMedicalHistoryByIDAsync(
        ID,
        cancellationToken);
}


public static async Task<ApiResult<MedicalHistoryDTO>> AddMedicalHistoryAsync(
    MedicalHistoryDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null)
    {
        return new ApiResult<MedicalHistoryDTO>
        {
            Data = null,
            Message = "MedicalHistory cannot be null.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await MedicalHistoryData.AddMedicalHistoryAsync(dto, cancellationToken);
}


public static async Task<ApiResult<MedicalHistoryUpdateDTO>> UpdateMedicalHistoryByIDAsync(
    MedicalHistoryUpdateDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null )
    {
        return new ApiResult<MedicalHistoryUpdateDTO>
        {
            Data = null,
            Message = "Invalid MedicalHistory data.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await MedicalHistoryData.UpdateMedicalHistoryByIDAsync(dto, cancellationToken);
}


public static async Task<ApiResult<MedicalHistoryDTO>> DeleteMedicalHistoryByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
   

    return await MedicalHistoryData.DeleteMedicalHistoryByIDAsync(
        ID,
        cancellationToken);
}

}

