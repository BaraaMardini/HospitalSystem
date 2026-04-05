
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public static class DoctorService
{

public static async Task<ApiResult<List<DoctorViewDTO>>> GetAllDoctorAsync(
    CancellationToken cancellationToken = default)
{
    var result = await DoctorData.GetAllDoctorAsync(cancellationToken);

    if (result.Data == null || result.Data.Count == 0)
    {
        return new ApiResult<List<DoctorViewDTO>>(
            null,
            "No Doctors found.",
            ErrorType.NotFound
        );
    }

    return new ApiResult<List<DoctorViewDTO>>(
        result.Data,
        "Doctors retrieved successfully.",
        ErrorType.None
    );
}



public static async Task<ApiResult<DoctorViewDTO>> GetDoctorByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
  
    return await DoctorData.GetDoctorByIDAsync(
        ID,
        cancellationToken);
}


public static async Task<ApiResult<DoctorDTO>> AddDoctorAsync(
    DoctorDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null)
    {
        return new ApiResult<DoctorDTO>
        {
            Data = null,
            Message = "Doctor cannot be null.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await DoctorData.AddDoctorAsync(dto, cancellationToken);
}


public static async Task<ApiResult<DoctorUpdateDTO>> UpdateDoctorByIDAsync(
    DoctorUpdateDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null )
    {
        return new ApiResult<DoctorUpdateDTO>
        {
            Data = null,
            Message = "Invalid Doctor data.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await DoctorData.UpdateDoctorByIDAsync(dto, cancellationToken);
}


public static async Task<ApiResult<DoctorDTO>> DeleteDoctorByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
   

    return await DoctorData.DeleteDoctorByIDAsync(
        ID,
        cancellationToken);
}

}

