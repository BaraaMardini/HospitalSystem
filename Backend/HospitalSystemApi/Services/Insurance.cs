
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public static class InsuranceService
{

public static async Task<ApiResult<List<InsuranceViewDTO>>> GetAllInsuranceAsync(
    CancellationToken cancellationToken = default)
{
    var result = await InsuranceData.GetAllInsuranceAsync(cancellationToken);

    if (result.Data == null || result.Data.Count == 0)
    {
        return new ApiResult<List<InsuranceViewDTO>>(
            null,
            "No Insurances found.",
            ErrorType.NotFound
        );
    }

    return new ApiResult<List<InsuranceViewDTO>>(
        result.Data,
        "Insurances retrieved successfully.",
        ErrorType.None
    );
}



public static async Task<ApiResult<InsuranceViewDTO>> GetInsuranceByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
  
    return await InsuranceData.GetInsuranceByIDAsync(
        ID,
        cancellationToken);
}


public static async Task<ApiResult<InsuranceDTO>> AddInsuranceAsync(
    InsuranceDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null)
    {
        return new ApiResult<InsuranceDTO>
        {
            Data = null,
            Message = "Insurance cannot be null.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await InsuranceData.AddInsuranceAsync(dto, cancellationToken);
}


public static async Task<ApiResult<InsuranceUpdateDTO>> UpdateInsuranceByIDAsync(
    InsuranceUpdateDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null )
    {
        return new ApiResult<InsuranceUpdateDTO>
        {
            Data = null,
            Message = "Invalid Insurance data.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await InsuranceData.UpdateInsuranceByIDAsync(dto, cancellationToken);
}


public static async Task<ApiResult<InsuranceDTO>> DeleteInsuranceByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
   

    return await InsuranceData.DeleteInsuranceByIDAsync(
        ID,
        cancellationToken);
}

}

