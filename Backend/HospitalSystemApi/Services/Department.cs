
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public static class DepartmentService
{

public static async Task<ApiResult<List<DepartmentViewDTO>>> GetAllDepartmentAsync(
    CancellationToken cancellationToken = default)
{
    var result = await DepartmentData.GetAllDepartmentAsync(cancellationToken);

    if (result.Data == null || result.Data.Count == 0)
    {
        return new ApiResult<List<DepartmentViewDTO>>(
            null,
            "No Departments found.",
            ErrorType.NotFound
        );
    }

    return new ApiResult<List<DepartmentViewDTO>>(
        result.Data,
        "Departments retrieved successfully.",
        ErrorType.None
    );
}



public static async Task<ApiResult<DepartmentViewDTO>> GetDepartmentByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
  
    return await DepartmentData.GetDepartmentByIDAsync(
        ID,
        cancellationToken);
}


public static async Task<ApiResult<DepartmentDTO>> AddDepartmentAsync(
    DepartmentDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null)
    {
        return new ApiResult<DepartmentDTO>
        {
            Data = null,
            Message = "Department cannot be null.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await DepartmentData.AddDepartmentAsync(dto, cancellationToken);
}


public static async Task<ApiResult<DepartmentUpdateDTO>> UpdateDepartmentByIDAsync(
    DepartmentUpdateDTO dto,
    CancellationToken cancellationToken = default)
{
    if (dto == null )
    {
        return new ApiResult<DepartmentUpdateDTO>
        {
            Data = null,
            Message = "Invalid Department data.",
            ErrorType = ErrorType.InvalidId
        };
    }

    return await DepartmentData.UpdateDepartmentByIDAsync(dto, cancellationToken);
}


public static async Task<ApiResult<DepartmentDTO>> DeleteDepartmentByIDAsync(
    int ID,
    CancellationToken cancellationToken = default)
{
   

    return await DepartmentData.DeleteDepartmentByIDAsync(
        ID,
        cancellationToken);
}

}

