
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public static class SpecializationService
{

public static async Task<ApiResult<List<SpecializationViewDTO>>> GetAllSpecializationAsync(
    CancellationToken cancellationToken = default)
{
    var result = await SpecializationData.GetAllSpecializationAsync(cancellationToken);

    if (result.Data == null || result.Data.Count == 0)
    {
        return new ApiResult<List<SpecializationViewDTO>>(
            null,
            "No Specializations found.",
            ErrorType.NotFound
        );
    }

    return new ApiResult<List<SpecializationViewDTO>>(
        result.Data,
        "Specializations retrieved successfully.",
        ErrorType.None
    );
}



public static async Task<ApiResult<SpecializationViewDTO>> GetSpecializationByidAsync(
    int id,
    CancellationToken cancellationToken = default)
{
  
    return await SpecializationData.GetSpecializationByidAsync(
        id,
        cancellationToken);
}


public static async Task<ApiResult<SpecializationDTO>> AddSpecializationAsync(
    SpecializationDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null)
    {
        return new ApiResult<SpecializationDTO>
        {
            Data = null,
            Message = "Specialization cannot be null.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await SpecializationData.AddSpecializationAsync(dto, cancellationToken);
}


public static async Task<ApiResult<SpecializationUpdateDTO>> UpdateSpecializationByidAsync(
    SpecializationUpdateDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null )
    {
        return new ApiResult<SpecializationUpdateDTO>
        {
            Data = null,
            Message = "Invalid Specialization data.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await SpecializationData.UpdateSpecializationByidAsync(dto, cancellationToken);
}


public static async Task<ApiResult<SpecializationDTO>> DeleteSpecializationByidAsync(
    int id,
    CancellationToken cancellationToken = default)
{
   

    return await SpecializationData.DeleteSpecializationByidAsync(
        id,
        cancellationToken);
}

}

